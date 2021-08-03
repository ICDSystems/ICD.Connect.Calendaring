using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Timers;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Calendaring.CalendarPoints;
using ICD.Connect.Calendaring.Comparers;
using ICD.Connect.Calendaring.Controls;

namespace ICD.Connect.Calendaring.CalendarManagers
{
	public sealed class CalendarManager : ICalendarManager
	{
		// Add 5 seconds to calculated reset time to avoid race conditions.
		private const int CURRENT_BOOKING_TIMER_RESET_BUFFER = 5 * 1000;
		// Max booking timer interval is 8 hours - prevents issues when no next booking is present
		private const long CURRENT_BOOKING_TIMER_MAX_INTERVAL = 8 * 60 * 60 * 1000;

		#region Events

		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		public event EventHandler OnBookingsChanged;

		/// <summary>
		/// Raised when the current (active) booking changes.
		/// </summary>
		public event EventHandler<GenericEventArgs<IBooking>> OnCurrentBookingChanged;

		#endregion

		private readonly SafeTimer m_CurrentBookingTimer;
		private readonly SafeCriticalSection m_CalendarSection;
		private readonly List<BookingGroup> m_Bookings;
		private readonly BiDictionary<ICalendarPoint, ICalendarControl> m_CalendarPointsToControls;

		private IBooking m_CachedCurrentBooking;
		
		

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public CalendarManager()
		{
			m_CalendarSection = new SafeCriticalSection();

			m_Bookings = new List<BookingGroup>();
			m_CalendarPointsToControls = new BiDictionary<ICalendarPoint, ICalendarControl>();

			m_CurrentBookingTimer = SafeTimer.Stopped(CurrentBookingTimerCallback);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the currently active booking if there is one.
		/// </summary>
		public IBooking CurrentBooking
		{
			get { return m_CachedCurrentBooking; }
			private set
			{
				if (value == m_CachedCurrentBooking)
					return;

				m_CachedCurrentBooking = value;

				m_CurrentBookingTimer.Reset(GetCurrentBookingTimerResetInterval());

				OnCurrentBookingChanged.Raise(this, m_CachedCurrentBooking);
			}
		}

		/// <summary>
		/// Gets the registered calendar providers.
		/// </summary>
		/// <returns></returns>
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
		public IEnumerable<BookingGroup> GetBookings()
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
		public void PushBooking(IBooking booking)
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
		public void CheckIn(IBooking booking)
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
		public void CheckOut(IBooking booking)
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
		public bool CanCheckIn(IBooking booking)
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
		public bool CanCheckOut(IBooking booking)
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
		public void RegisterCalendarProvider(ICalendarPoint calendarPoint)
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
		public void DeregisterCalendarProvider(ICalendarPoint calendarPoint)
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
			foreach (ICalendarPoint point in m_CalendarSection.Execute(() => m_CalendarPointsToControls.Keys.ToArray()))
				DeregisterCalendarProvider(point);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// De-Duplicates the booking collection.
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<BookingGroup> DeduplicateBookings(IEnumerable<IBooking> bookings)
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
					.OrderBy(b => b.StartTime);

				BookingGroup[] deduplicatedBookings = DeduplicateBookings(newBookings).ToArray();
				if (deduplicatedBookings.SequenceEqual(m_Bookings, BookingEqualityComparer.Instance.Equals))
					return;

				m_Bookings.Clear();
				m_Bookings.AddRange(deduplicatedBookings);
			}
			finally
			{
				m_CalendarSection.Leave();
			}

			UpdateCurrentBooking();

			OnBookingsChanged.Raise(this);
		}

		/// <summary>
		/// Raises the current booking changed event if the current booking has changed
		/// </summary>
		private void CurrentBookingTimerCallback()
		{
			UpdateCurrentBooking();
		}

		private void UpdateCurrentBooking()
		{
			CurrentBooking = GetBookings().FirstOrDefault(BookingExtensions.IsBookingCurrent);
		}

		private long GetCurrentBookingTimerResetInterval()
		{
			var nextStart = this.GetTimeToNextBookingStart();
			var nextEnd = this.GetTimeToNextBookingEnd();

			return (long)(Math.Min(Math.Min(nextStart.TotalMilliseconds, nextEnd.TotalMilliseconds) +
			              CURRENT_BOOKING_TIMER_RESET_BUFFER, CURRENT_BOOKING_TIMER_MAX_INTERVAL));
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

		#region Console

		public string ConsoleName { get { return "CalendarManager"; } }
		public string ConsoleHelp { get { return ""; } }

		public IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			yield break;
		}

		public void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
		}

		public IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			yield return new ConsoleCommand("PrintBookingGroups", "Prints the booking groups", () => PrintBookingGroups());
		}

		private string PrintBookingGroups()
		{
			TableBuilder builder =
				new TableBuilder("Meeting Name",
				                 "Organizer Name",
				                 "Organizer Email",
				                 "Start Time",
				                 "End Time",
				                 "IsPrivate",
				                 "Protocol",
				                 "Call Type",
				                 "Number");

			foreach (BookingGroup bookingGroup in GetBookings())
			{
				foreach (IBooking booking in bookingGroup.GetUnderlyingBookings())
				{
					string protocol = string.Join(IcdEnvironment.NewLine,
					                              booking.GetBookingNumbers()
					                                     .Select(c => StringUtils.NiceName(c.Protocol))
					                                     .ToArray());

					string callType = string.Join(IcdEnvironment.NewLine,
					                              booking.GetBookingNumbers()
					                                     .Select(c => c.CallType.ToString())
					                                     .ToArray());

					string number = string.Join(IcdEnvironment.NewLine,
					                            booking.GetBookingNumbers()
					                                   .Select(c => c.DialString)
					                                   .ToArray());

					builder.AddRow(booking.MeetingName,
					               booking.OrganizerName,
					               booking.OrganizerEmail,
					               booking.StartTime.ToLocalTime(),
					               booking.EndTime.ToLocalTime(),
					               booking.IsPrivate,
					               protocol,
					               callType,
					               number);
				}

				builder.AddSeparator();
			}

			return builder.ToString();
		}

		#endregion
	}
}
