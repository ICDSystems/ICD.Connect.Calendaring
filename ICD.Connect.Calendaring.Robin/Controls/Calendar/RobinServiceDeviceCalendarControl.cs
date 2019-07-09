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
using ICD.Connect.Calendaring.Robin.Components.Events;
using ICD.Connect.Conferencing.DialContexts;

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
		private readonly IcdOrderedDictionary<Event, RobinBooking> m_EventToBooking;
		private readonly SafeCriticalSection m_BookingSection;

		/// <summary>
		/// Sort events by start time.
		/// </summary>
		private static readonly PredicateComparer<Event, DateTime> s_EventComparer;

		/// <summary>
		/// Static constructor.
		/// </summary>
		static RobinServiceDeviceCalendarControl()
		{
			s_EventComparer = new PredicateComparer<Event, DateTime>(e => e.MeetingStart.DateTimeInfo);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public RobinServiceDeviceCalendarControl(RobinServiceDevice parent, int id)
			: base(parent, id)
		{
			m_EventToBooking = new IcdOrderedDictionary<Event, RobinBooking>(s_EventComparer);
			m_BookingSection = new SafeCriticalSection();

			m_EventsComponent = Parent.Components.GetComponent<EventsComponent>();
			Subscribe(m_EventsComponent);

			m_RefreshTimer = new SafeTimer(Refresh, TIMER_REFRESH_INTERVAL, TIMER_REFRESH_INTERVAL);
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
			return m_BookingSection.Execute(() => m_EventToBooking.Values.ToArray(m_EventToBooking.Count));
		}

		#endregion

		#region Parent Callbacks

		/// <summary>
		/// Subscribe to the events events.
		/// </summary>
		/// <param name="events"></param>
		private void Subscribe(EventsComponent events)
		{
			events.OnEventsUpdated += EventsOnEventsUpdated;
		}

		/// <summary>
		/// Unsubscribe from the events events.
		/// </summary>
		/// <param name="events"></param>
		private void Unsubscribe(EventsComponent events)
		{
			events.OnEventsUpdated -= EventsOnEventsUpdated;
		}

		/// <summary>
		/// Called when events are added/removed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EventsOnEventsUpdated(object sender, EventArgs e)
		{
			bool change = false;

			Event[] events =
				m_EventsComponent.GetEvents()
				                 .Where(b => b.MeetingEnd.DateTimeInfo > IcdEnvironment.GetLocalTime())
				                 .Distinct()
				                 .ToArray();

			m_BookingSection.Enter();

			try
			{
				IcdHashSet<Event> existing = m_EventToBooking.Keys.ToIcdHashSet();
				IcdHashSet<Event> removeEventsList = existing.Subtract(events);

				foreach (Event @event in removeEventsList)
					change |= RemoveEvent(@event);

				foreach (Event @event in events)
					change |= AddEvent(@event);
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

		private bool AddEvent(Event @event)
		{
			if (@event == null)
				throw new ArgumentNullException("event");

			m_BookingSection.Enter();

			try
			{
				if (m_EventToBooking.ContainsKey(@event))
					return false;

				IEnumerable<IDialContext> dialContexts = Parent.CalendarParserCollection.ParseText(@event.Description);
				RobinBooking robinBooking = new RobinBooking(@event, dialContexts);

				m_EventToBooking.Add(@event, robinBooking);
			}
			finally
			{
				m_BookingSection.Leave();
			}

			return true;
		}

		private bool RemoveEvent(Event @event)
		{
			if (@event == null)
				throw new ArgumentNullException("event");

			return m_BookingSection.Execute(() => m_EventToBooking.Remove(@event));
		}

		#endregion
	}
}
