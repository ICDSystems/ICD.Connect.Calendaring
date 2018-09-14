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

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar
{
    public sealed class RobinServiceDeviceCalendarControl : AbstractCalendarControl<RobinServiceDevice>
    {
        private const int TIMERREFRESHINTERVAL = 10 * 60 * 1000;

        private readonly BookingsComponent m_BookingsComponent;
	    private readonly SafeTimer m_RefreshTimer;
	    private readonly List<RobinBooking> m_SortedBookings;
	    private readonly IcdHashSet<RobinBooking> m_HashBooking;

	    /// <summary>
	    /// Raised when bookings are added/removed.
	    /// </summary>
	    public override event EventHandler OnBookingsChanged;

		/// <summary>
		/// Sort bookings by start time.
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
		    m_RefreshTimer = new SafeTimer(Refresh, TIMERREFRESHINTERVAL, TIMERREFRESHINTERVAL);

		    m_SortedBookings = new List<RobinBooking>();
		    m_HashBooking = new IcdHashSet<RobinBooking>(new BookingsComparer<RobinBooking>());

		    m_BookingsComponent = Parent.Components.GetComponent<BookingsComponent>();
		    Subscribe(m_BookingsComponent);
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

			Unsubscribe(m_BookingsComponent);
	    }

		/// <summary>
		/// Subscribe to the bookings events.
		/// </summary>
		/// <param name="bookings"></param>
	    private void Subscribe(BookingsComponent bookings)
	    {
		    bookings.OnBookingsUpdated += BookingsOnOnBookingsUpdated;
	    }

		/// <summary>
		/// Unsubscribe from the bookings events.
		/// </summary>
		/// <param name="bookings"></param>
	    private void Unsubscribe(BookingsComponent bookings)
	    {
		    bookings.OnBookingsUpdated -= BookingsOnOnBookingsUpdated;
	    }

	    /// <summary>
	    /// Called when bookings are added/removed.
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    private void BookingsOnOnBookingsUpdated(object sender, EventArgs e)
	    {
		    bool change = false;

		    Components.Bookings.Booking[] bookings = m_BookingsComponent.GetBookings()
			    //.Where(b => b.EndTime > IcdEnvironment.GetLocalTime())
			    .Distinct()
			    .ToArray();
		    IcdHashSet<RobinBooking> existing = m_SortedBookings.ToIcdHashSet(new BookingsComparer<RobinBooking>());
		    IcdHashSet<RobinBooking> current = bookings.Select(b => new RobinBooking(b)).ToIcdHashSet(new BookingsComparer<RobinBooking>());

		    IcdHashSet<RobinBooking> removeBookingList = existing.Subtract(current);
		    foreach (RobinBooking booking in removeBookingList)
			    change |= RemoveBooking(booking);

		    foreach (var booking in bookings)
			    change |= AddBooking(booking);

		    if (change)
			    OnBookingsChanged.Raise(this);

	    }

	    public override void Refresh()
	    {
			m_BookingsComponent.UpdateBookings();
	    }

		public override IEnumerable<IBooking> GetBookings()
	    {
		    return m_SortedBookings.ToArray(m_SortedBookings.Count);
	    }

	    private bool AddBooking(Components.Bookings.Booking booking)
	    {
		    if (booking == null)
			    throw new ArgumentNullException("booking");

		    RobinBooking zoomBooking = new RobinBooking(booking);

		    if (m_HashBooking.Contains(zoomBooking))
			    return false;

		    m_HashBooking.Add(zoomBooking);

		    m_SortedBookings.AddSorted(zoomBooking, s_BookingComparer);

		    return true;
	    }

	    private bool RemoveBooking(RobinBooking zoomBooking)
	    {
		    if (!m_HashBooking.Contains(zoomBooking))
			    return false;

		    m_HashBooking.Remove(zoomBooking);
		    m_SortedBookings.Remove(zoomBooking);

		    return true;
	    }
    }
}
