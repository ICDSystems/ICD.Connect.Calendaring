using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Calendaring.CalendarPoints;
using ICD.Connect.Calendaring.Comparers;
using ICD.Connect.Calendaring.Controls;

namespace ICD.Connect.Calendaring.CalendarManagers
{
	public sealed class CalendarManager : ICalendarManager
	{
		#region Events

		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		public event EventHandler OnBookingsChanged;

		#endregion

		private readonly SafeCriticalSection m_CalendarSection;
		private readonly List<IBooking> m_Bookings;
		private readonly BiDictionary<ICalendarPoint, ICalendarControl> m_CalendarPointsToControls;

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public CalendarManager()
		{
			m_CalendarSection = new SafeCriticalSection();

			m_Bookings = new List<IBooking>();
			m_CalendarPointsToControls = new BiDictionary<ICalendarPoint, ICalendarControl>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the registered calendar providers.
		/// </summary>
		/// <returns></returns>
		[NotNull]
		public IEnumerable<ICalendarControl> GetProviders()
		{
			m_CalendarSection.Enter();

			try
			{
				return m_CalendarPointsToControls
					.OrderBy(kvp => kvp.Key.Order)
					.Select(kvp => kvp.Value)
					.ToArray();
			}
			finally
			{
				m_CalendarSection.Leave();
			}
		}

		/// <summary>
		/// Gets the registered calendar providers where both the calendar point and calendar control
		/// instersect with the given features mask.
		/// </summary>
		/// <returns></returns>
		[NotNull]
		public IEnumerable<ICalendarControl> GetProviders(eCalendarFeatures features)
		{
			m_CalendarSection.Enter();

			try
			{
				return m_CalendarPointsToControls
					.Where(kvp =>
					       kvp.Key
					          .Features
					          .HasFlags(features) &&
					       kvp.Value
					          .SupportedCalendarFeatures
					          .HasFlags(features))
					.OrderBy(kvp => kvp.Key.Order)
					.Select(kvp => kvp.Value)
					.ToArray();
			}
			finally
			{
				m_CalendarSection.Leave();
			}
		}

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
		/// Pushes the specified booking to the providers.
		/// </summary>
		/// <param name="booking"></param>
		public void PushBooking([NotNull] IBooking booking)
		{
			if (booking == null)
				throw new ArgumentNullException("booking");

			foreach (ICalendarControl control in GetProviders(eCalendarFeatures.CreateBookings))
				control.PushBooking(booking);
		}

		/// <summary>
		/// Checks into the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		public void CheckIn([NotNull] IBooking booking)
		{
			if (booking == null)
				throw new ArgumentNullException("booking");

			BookingGroup bg = booking as BookingGroup;
			if (bg != null)
			{
				bg.ForEach(CheckIn);
				return;
			}

			foreach (ICalendarControl control in GetProviders(eCalendarFeatures.CheckIn).Where(c => c.CanCheckIn(booking)))
				control.CheckIn(booking);
		}

		/// <summary>
		/// Checks out of the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		public void CheckOut([NotNull] IBooking booking)
		{
			if (booking == null)
				throw new ArgumentNullException("booking");

			BookingGroup bg = booking as BookingGroup;
			if (bg != null)
			{
				bg.ForEach(CheckOut);
				return;
			}

			foreach (ICalendarControl control in GetProviders(eCalendarFeatures.CheckOut).Where(c => c.CanCheckOut(booking)))
				control.CheckOut(booking);
		}

		/// <summary>
		/// Determines if the specified booking can be checked into.
		/// </summary>
		/// <param name="booking"></param>
		public bool CanCheckIn([NotNull] IBooking booking)
		{
			if (booking == null)
				throw new ArgumentNullException("booking");

			BookingGroup bg = booking as BookingGroup;
			if (bg != null)
				return bg.Any(CanCheckIn);

			return GetProviders(eCalendarFeatures.CheckIn).Any(c => c.CanCheckIn(booking));
		}

		/// <summary>
		/// Determines if the specified booking can be checked out of.
		/// </summary>
		/// <param name="booking"></param>
		/// <returns></returns>
		public bool CanCheckOut([NotNull] IBooking booking)
		{
			if (booking == null)
				throw new ArgumentNullException("booking");

			BookingGroup bg = booking as BookingGroup;
			if (bg != null)
				return bg.Any(CanCheckOut);

			return GetProviders(eCalendarFeatures.CheckOut).Any(c => c.CanCheckOut(booking));
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

			m_CalendarSection.Enter();

			try
			{
				ICalendarControl control;
				if (m_CalendarPointsToControls.TryGetValue(calendarPoint, out control) && control == calendarPoint.Control)
					return;

				Unsubscribe(control);
				m_CalendarPointsToControls.Add(calendarPoint, calendarPoint.Control);
				Subscribe(calendarPoint.Control);
			}

			finally
			{
				m_CalendarSection.Leave();
			}

			UpdateBookings();
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

			m_CalendarSection.Enter();

			try
			{
				ICalendarControl control;
				if (!m_CalendarPointsToControls.TryGetValue(calendarPoint, out control))
					return;

				Unsubscribe(control);
				m_CalendarPointsToControls.RemoveKey(calendarPoint);
			}
			finally
			{
				m_CalendarSection.Leave();
			}

			UpdateBookings();
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
			                        (b, bs) => new BookingGroup(bs),
			                        BookingDeduplicationEqualityComparer.Instance)
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
				IEnumerable<IBooking> newBookings = GetProviders(eCalendarFeatures.ListBookings)
					.SelectMany(c => c.GetBookings())
					.OrderBy(b => b.StartTime)
					.ToArray();

				newBookings = DeduplicateBookings(newBookings).ToArray();

				if (newBookings.SequenceEqual(m_Bookings, BookingDeduplicationEqualityComparer.Instance))
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

		#endregion

		#region Calendar Control Callbacks

		/// <summary>
		/// Subscribe to the control events.
		/// </summary>
		/// <param name="control"></param>
		private void Subscribe([CanBeNull] ICalendarControl control)
		{
			if (control == null)
				return;

			control.OnBookingsChanged += ControlOnBookingsChanged;
		}

		/// <summary>
		/// Unsubscribe from the control events.
		/// </summary>
		/// <param name="control"></param>
		private void Unsubscribe([CanBeNull] ICalendarControl control)
		{
			if (control == null)
				return;

			control.OnBookingsChanged -= ControlOnBookingsChanged;
		}

		/// <summary>
		/// Called when a calendar control raises new bookings.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ControlOnBookingsChanged(object sender, EventArgs e)
		{
			UpdateBookings();
		}

		#endregion
	}
}
