using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Comparers;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Timers;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.CalendarControl;
using ICD.Connect.Calendaring.Comparers;
using ICD.Connect.Calendaring.Robin.Components.Events;
using ICD.Connect.Calendaring.Robin.Controls.Calendar.Bookings;

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar
{
	public sealed class RobinServiceDeviceCalendarControl : AbstractCalendarControl<RobinServiceDevice>
	{
		private const int TIMER_REFRESH_INTERVAL = 10 * 60 * 1000;

		/// <summary>
		/// Raised when events are added/removed.
		/// </summary>
		public override event EventHandler OnBookingsChanged;

		private readonly EventsComponent m_EventsComponent;
		private readonly SafeTimer m_RefreshTimer;
		private readonly List<RobinBooking> m_SortedBookings;
		private readonly IcdHashSet<RobinBooking> m_HashBooking;
		private readonly SafeCriticalSection m_BookingSection;

		/// <summary>
		/// Sort events by start time.
		/// </summary>
		private static readonly PredicateComparer<RobinBooking, DateTime> s_BookingComparer;

		/// <summary>
		/// Static constructor.
		/// </summary>
		static RobinServiceDeviceCalendarControl()
		{
			s_BookingComparer = new PredicateComparer<RobinBooking, DateTime>(b => b.StartTime);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public RobinServiceDeviceCalendarControl(RobinServiceDevice parent, int id)
			: base(parent, id)
		{
			m_RefreshTimer = new SafeTimer(Refresh, TIMER_REFRESH_INTERVAL, TIMER_REFRESH_INTERVAL);

			m_SortedBookings = new List<RobinBooking>();
			m_HashBooking = new IcdHashSet<RobinBooking>(new BookingsComparer<RobinBooking>());

			m_BookingSection = new SafeCriticalSection();

			m_EventsComponent = Parent.Components.GetComponent<EventsComponent>();
			Subscribe(m_EventsComponent);
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnBookingsChanged = null;

			m_RefreshTimer.Dispose();

			base.DisposeFinal(disposing);

			Unsubscribe(m_EventsComponent);
		}

		#region Methods

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override void Refresh()
		{
			m_EventsComponent.UpdateBookings();
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override IEnumerable<IBooking> GetBookings()
		{
			return m_BookingSection.Execute(() => m_SortedBookings.ToArray(m_SortedBookings.Count));
		}

		#endregion

		#region Parent Callbacks

		/// <summary>
		/// Subscribe to the events events.
		/// </summary>
		/// <param name="events"></param>
		private void Subscribe(EventsComponent events)
		{
			events.OnEventsUpdated += EventsOnOnEventsUpdated;
		}

		/// <summary>
		/// Unsubscribe from the events events.
		/// </summary>
		/// <param name="events"></param>
		private void Unsubscribe(EventsComponent events)
		{
			events.OnEventsUpdated -= EventsOnOnEventsUpdated;
		}

		/// <summary>
		/// Called when events are added/removed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EventsOnOnEventsUpdated(object sender, EventArgs e)
		{
			bool change = false;

			Event[] events = m_EventsComponent.GetEvents()
			                                  .Where(b => b.MeetingEnd.DateTimeInfo > IcdEnvironment.GetLocalTime())
			                                  .Distinct()
			                                  .ToArray();

			m_BookingSection.Enter();

			try
			{
				IcdHashSet<RobinBooking> existing = m_SortedBookings.ToIcdHashSet(new BookingsComparer<RobinBooking>());
				IcdHashSet<RobinBooking> current =
					events.Select(b => new RobinBooking(b, Parent.CalendarParserCollection.ParseText(b.Description)))
					      .ToIcdHashSet(new BookingsComparer<RobinBooking>());

				IcdHashSet<RobinBooking> removeBookingList = existing.Subtract(current);
				foreach (RobinBooking booking in removeBookingList)
					change |= RemoveBooking(booking);

				foreach (Event @event in events)
					change |= AddBooking(@event);
			}
			finally
			{
				m_BookingSection.Leave();
			}

			if (change)
				OnBookingsChanged.Raise(this);
		}

		#endregion

		#region Private Methods

		private bool AddBooking(Event @event)
		{
			m_BookingSection.Enter();

			try
			{
				IEnumerable<BookingProtocolInfo> bookingNumbers = Parent.CalendarParserCollection.ParseText(@event.Description);
				RobinBooking robinBooking = new RobinBooking(@event, bookingNumbers);

				if (m_HashBooking.Contains(robinBooking))
					return false;

				m_HashBooking.Add(robinBooking);

				m_SortedBookings.AddSorted(robinBooking, s_BookingComparer);

			}
			finally
			{
				m_BookingSection.Leave();
			}

			return true;
		}

		private bool RemoveBooking(RobinBooking robinBooking)
		{
			m_BookingSection.Enter();

			try
			{
				if (!m_HashBooking.Contains(robinBooking))
					return false;

				m_HashBooking.Remove(robinBooking);
				m_SortedBookings.Remove(robinBooking);
			}
			finally
			{
				m_BookingSection.Leave();
			}

			return true;
		}

		#endregion
	}
}
