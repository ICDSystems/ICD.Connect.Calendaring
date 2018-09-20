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
using ICD.Connect.Calendaring.Robin.Components.Bookings;
using ICD.Connect.Calendaring.Robin.Controls.Calendar.Bookings;

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar
{
    public sealed class RobinServiceDeviceCalendarControl : AbstractCalendarControl<RobinServiceDevice>
    {
        private const int TIMER_REFRESH_INTERVAL = 10 * 60 * 1000;

        private readonly EventsComponent m_EventsComponent;
	    private readonly SafeTimer m_RefreshTimer;
	    private readonly List<IRobinBooking> m_SortedBookings;
	    private readonly IcdHashSet<IRobinBooking> m_HashBooking;
	    private readonly RobinServiceDevice m_Parent;

		/// <summary>
		/// Raised when events are added/removed.
		/// </summary>
		public override event EventHandler OnBookingsChanged;

		/// <summary>
		/// Sort events by start time.
		/// </summary>
		private static readonly PredicateComparer<IRobinBooking, DateTime> s_BookingComparer;

		/// <summary>
		/// Static constructor.
		/// </summary>
		static RobinServiceDeviceCalendarControl()
	    {
			s_BookingComparer = new PredicateComparer<IRobinBooking, DateTime>(b => b.StartTime);
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

		    m_SortedBookings = new List<IRobinBooking>();
		    m_HashBooking = new IcdHashSet<IRobinBooking>(new BookingsComparer<IRobinBooking>());
		    m_Parent = parent;

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
		    IcdHashSet<IRobinBooking> existing = m_SortedBookings.ToIcdHashSet(new BookingsComparer<IRobinBooking>());
		    IcdHashSet<IRobinBooking> current = events.Select(b => GetBookingProtocol(b)).ToIcdHashSet(new BookingsComparer<IRobinBooking>());

		    IcdHashSet<IRobinBooking> removeBookingList = existing.Subtract(current);
		    foreach (IRobinBooking booking in removeBookingList)
			    change |= RemoveBooking(booking);

		    foreach (var booking in events)
			    change |= AddBooking(booking);

		    if (change)
			    OnBookingsChanged.Raise(this);

	    }

		public override void Refresh()
	    {
			m_EventsComponent.UpdateBookings();
	    }

		public override IEnumerable<IBooking> GetBookings()
	    {
		    return m_SortedBookings.ToArray(m_SortedBookings.Count);
	    }

	    private bool AddBooking(Event @event)
	    {
			IRobinBooking robinBooking = GetBookingProtocol(@event);

			if (m_HashBooking.Contains(robinBooking))
			    return false;

		    m_HashBooking.Add(robinBooking);

		    m_SortedBookings.AddSorted(robinBooking, s_BookingComparer);

		    return true;
	    }

	    private bool RemoveBooking(IRobinBooking robinBooking)
	    {
		    if (!m_HashBooking.Contains(robinBooking))
			    return false;

		    m_HashBooking.Remove(robinBooking);
		    m_SortedBookings.Remove(robinBooking);

		    return true;
	    }

	    private IRobinBooking GetBookingProtocol(Event @event)
	    {
		    if (@event == null)
			    throw new ArgumentNullException("event");

		    BookingProtocolInfo info = BookingParsingUtils.GetProtocolInfo(@event.Description);

		    IRobinBooking robinBooking;

		    switch (info.BookingProtocol)
		    {
			    case eBookingProtocol.None:
				    robinBooking = new RobinBooking(@event);
				    break;
			    case eBookingProtocol.Sip:
					throw new NotSupportedException();
			    case eBookingProtocol.Pstn:
					throw new NotSupportedException();
				case eBookingProtocol.Zoom:
					robinBooking = new ZoomBooking(@event) {MeetingNumber = info.Number};
					break;
			    default:
				    throw new ArgumentOutOfRangeException();
		    }

		    return robinBooking;
	    }
    }
}
