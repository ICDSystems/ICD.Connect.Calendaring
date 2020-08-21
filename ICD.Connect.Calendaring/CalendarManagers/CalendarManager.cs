using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Calendaring.CalendarPoints;
using ICD.Connect.Calendaring.Controls;

namespace ICD.Connect.Calendaring.CalendarManagers
{
	public sealed class CalendarManager : ICalendarManager
	{
		#region Events

		public event EventHandler OnBookingsChanged;

		#endregion

		private readonly SafeCriticalSection m_CalendarSection;

		private readonly List<IBooking> m_Bookings;
		private readonly BiDictionary<ICalendarPoint, ICalendarControl> m_CalendarPointsToControls;

		#region Constructor

		public CalendarManager()
		{
			m_CalendarSection = new SafeCriticalSection();

			m_Bookings = new List<IBooking>();
			m_CalendarPointsToControls = new BiDictionary<ICalendarPoint, ICalendarControl>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the available bookings.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IBooking> GetBookings()
		{
			return m_CalendarSection.Execute(() => m_Bookings.ToArray());
		}

		/// <summary>
		/// Refreshes the collection of bookings on the manager.
		/// </summary>
		public void RefreshBookings()
		{
			m_CalendarSection.Execute(() => m_CalendarPointsToControls.Values.ForEach(c => c.Refresh()));
		}

		/// <summary>
		/// Pushes the specified booking to the most appropriate provider.
		/// </summary>
		/// <param name="booking"></param>
		public void PushBooking(IBooking booking)
		{
			var provider = FindBestProviderForNewBooking();

			if (provider == null)
				throw new InvalidOperationException("No provider found to push booking to");

			provider.PushBooking(booking);
		}

		/// <summary>
		/// Checks into the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		public void CheckIn(IBooking booking)
		{
			var provider = FindBestProviderForExistingBooking(booking);

			if (provider == null)
				throw new InvalidOperationException("No provider found to check in booking");

			var bg = booking as BookingGroup;
			provider.CheckIn(bg == null ? booking : bg.Key);
		}

		/// <summary>
		/// Checks out of the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		public void CheckOut(IBooking booking)
		{
			var provider = FindBestProviderForExistingBooking(booking);

			if (provider == null)
				throw new InvalidOperationException("No provider found to check out booking");

			var bg = booking as BookingGroup;
			provider.CheckOut(bg == null ? booking : bg.Key);
		}

		/// <summary>
		/// Determines if the specified booking can be checked into.
		/// </summary>
		/// <param name="booking"></param>
		public bool CanCheckIn(IBooking booking)
		{
			return m_CalendarSection.Execute(() => m_CalendarPointsToControls.Values.Any(c => c.CanCheckIn(booking)));
		}

		/// <summary>
		/// Determines if the specified booking can be checked out of.
		/// </summary>
		/// <param name="booking"></param>
		/// <returns></returns>
		public bool CanCheckOut(IBooking booking)
		{
			return m_CalendarSection.Execute(() => m_CalendarPointsToControls.Values.Any(c => c.CanCheckOut(booking)));
		}

		/// <summary>
		/// Registers the calendar provider at the given calendar point.
		/// </summary>
		/// <param name="calendarPoint"></param>
		public void RegisterCalendarProvider([NotNull] ICalendarPoint calendarPoint)
		{
			if (calendarPoint == null)
				throw new ArgumentNullException("calendarPoint");

			if (calendarPoint.Control == null)
				throw new ArgumentNullException("calendarPoint", "Calendar Point has no calendar control");

			RegisterCalendarProvider(calendarPoint, calendarPoint.Control);
		}

		/// <summary>
		/// Deregisters the calendar provider at the given calendar point.
		/// </summary>
		/// <param name="calendarPoint"></param>
		public void DeregisterCalendarProvider([NotNull] ICalendarPoint calendarPoint)
		{
			if (calendarPoint == null)
				throw new ArgumentNullException("calendarPoint");

			if (calendarPoint.Control == null)
				throw new ArgumentNullException("calendarPoint", "Calendar Point has no calendar control");

			DeregisterCalendarProvider(calendarPoint, calendarPoint.Control);
		}

		/// <summary>
		/// Sets the calendar manager back to it's initial state.
		/// </summary>
		public void Clear()
		{
			m_CalendarSection.Enter();

			try
			{
				foreach (ICalendarPoint point in m_CalendarPointsToControls.Keys)
					DeregisterCalendarProvider(point);

				m_Bookings.Clear();
				m_CalendarPointsToControls.Clear();
			}
			finally
			{
				m_CalendarSection.Leave();
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// De-Duplicates the booking collection.
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<IBooking> DeduplicateBookings(IEnumerable<IBooking> bookings)
		{
			return bookings.GroupBy(x => x,
			                        (b, bs) =>
				                        new BookingGroup(bs),
			                        BookingEqualityComparer.Instance)
			               .ToArray();
		}

		/// <summary>
		/// Updates the collection of bookings.
		/// </summary>
		private void UpdateBookings()
		{
			m_CalendarSection.Enter();

			try
			{
				IEnumerable<IBooking> newBookings =
					m_CalendarPointsToControls
						.Where(c => c.Value.SupportedCalendarFeatures.HasFlag(eCalendarFeatures.ListBookings))
						.OrderBy(kvp => kvp.Key.Order)
						.SelectMany(c => c.Value.GetBookings())
						.OrderBy(b => b.StartTime)
						.ToArray();

				newBookings = DeduplicateBookings(newBookings).ToArray();

				if (newBookings.SequenceEqual(m_Bookings, BookingEqualityComparer.Instance))
					return;

				m_Bookings.Clear();
				m_Bookings.AddRange(newBookings);
			}
			finally
			{
				m_CalendarSection.Leave();
			}

			OnBookingsChanged.Raise(this);
		}

		/// <summary>
		/// Finds the calendar control associated with the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		/// <returns></returns>
		[CanBeNull]
		private ICalendarControl FindBestProviderForExistingBooking([NotNull] IBooking booking)
		{
			m_CalendarSection.Enter();

			try
			{
				var bg = booking as BookingGroup;
				return bg != null
					       ? m_CalendarPointsToControls.Values.FirstOrDefault(c => c.GetBookings().Contains(bg.Key))
					       : m_CalendarPointsToControls.Values.FirstOrDefault(c => c.GetBookings().Contains(booking));
			}
			finally
			{
				m_CalendarSection.Leave();
			}
		}

		[CanBeNull]
		private ICalendarControl FindBestProviderForNewBooking()
		{
			return m_CalendarSection.Execute(() => m_CalendarPointsToControls
			                                       .Where(kvp => 
				                                              kvp.Value
				                                                 .SupportedCalendarFeatures
				                                                 .HasFlag(eCalendarFeatures.CreateBookings))
			                                       .OrderBy(kvp => kvp.Key.Order)
												   .Select(kvp => kvp.Value)
			                                       .FirstOrDefault());
		}

		private void RegisterCalendarProvider([NotNull] ICalendarPoint point, [NotNull] ICalendarControl control)
		{
			m_CalendarSection.Enter();

			try
			{
				ICalendarControl oldControl;
				if (m_CalendarPointsToControls.TryGetValue(point, out oldControl) && oldControl == control)
					return;

				Unsubscribe(control);
				m_CalendarPointsToControls.Add(point, control);
				Subscribe(control);
			}
			finally
			{
				m_CalendarSection.Leave();
			}

			UpdateBookings();
		}

		private void DeregisterCalendarProvider([NotNull] ICalendarPoint point, [NotNull] ICalendarControl control)
		{
			m_CalendarSection.Enter();

			try
			{
				if (!m_CalendarPointsToControls.ContainsKey(point))
					return;

				Unsubscribe(control);
				m_CalendarPointsToControls.RemoveKey(point);
			}
			finally
			{
				m_CalendarSection.Leave();
			}

			UpdateBookings();
		}

		#endregion

		#region Calendar Control Callbacks

		private void Subscribe(ICalendarControl control)
		{
			control.OnBookingsChanged += ControlOnBookingsChanged;
		}

		private void Unsubscribe(ICalendarControl control)
		{
			control.OnBookingsChanged -= ControlOnBookingsChanged;
		}

		private void ControlOnBookingsChanged(object sender, EventArgs e)
		{
			UpdateBookings();
		}

		#endregion
	}
}
