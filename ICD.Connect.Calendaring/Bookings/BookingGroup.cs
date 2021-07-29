using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Bookings
{
    public sealed class BookingGroup : AbstractBooking, IGrouping<IBooking, IBooking>
    {
	    private readonly List<IBooking> m_Bookings;

	    #region Properties

	    public IBooking Key { get { return m_Bookings.FirstOrDefault(); } }

	    /// <summary>
	    /// Returns the name of the meeting.
	    /// </summary>
	    public override string MeetingName
	    {
		    get
		    {
			    return m_Bookings.Select(b => b.MeetingName)
			                     .FirstOrDefault(n => !string.IsNullOrEmpty(n));
		    }
	    }

	    /// <summary>
	    /// Returns the organizer's name.
	    /// </summary>
	    public override string OrganizerName
	    {
		    get
		    {
			    return m_Bookings.Select(b => b.OrganizerName)
			                     .FirstOrDefault(n => !string.IsNullOrEmpty(n));
		    }
	    }

	    /// <summary>
	    /// Returns the organizer's email.
	    /// </summary>
	    public override string OrganizerEmail
	    {
		    get
		    {
			    return m_Bookings.Select(b => b.OrganizerEmail)
			                     .FirstOrDefault(n => !string.IsNullOrEmpty(n));
		    }
	    }

	    /// <summary>
	    /// Returns the meeting start time.
	    /// </summary>
	    public override DateTime StartTime { get { return Key == null ? DateTime.MinValue : Key.StartTime; } }

	    /// <summary>
	    /// Returns the meeting end time.
	    /// </summary>
	    public override DateTime EndTime { get { return Key == null ? DateTime.MaxValue : Key.EndTime; } }

	    /// <summary>
	    /// Returns true if meeting is private.
	    /// </summary>
	    public override bool IsPrivate { get { return Key != null && Key.IsPrivate; } }

	    /// <summary>
	    /// Returns true if the booking is checked in.
	    /// </summary>
	    public override bool CheckedIn { get { return Key != null && Key.CheckedIn; } }

	    /// <summary>
	    /// Returns true if the booking is checked out.
	    /// </summary>
	    public override bool CheckedOut { get { return Key != null && Key.CheckedOut; } }

	    #endregion

	    #region Constructor

	    /// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="bookings"></param>
	    public BookingGroup([NotNull] IEnumerable<IBooking> bookings)
        {
	        m_Bookings = bookings.ToList();
        }

	    #endregion

	    #region Methods

	    /// <summary>
	    /// Returns Booking Numbers.
	    /// </summary>
	    public override IEnumerable<IDialContext> GetBookingNumbers()
        {
	        return m_Bookings.SelectMany(b => b.GetBookingNumbers())
	                         .Distinct(DialContextEqualityComparer.Instance);
        }

		public IEnumerable<IBooking> GetUnderlyingBookings()
		{
			return m_Bookings;
		}

	    #endregion

	    #region Enumerators

	    /// <summary>Returns an enumerator that iterates through the collection.</summary>
	    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
	    public IEnumerator<IBooking> GetEnumerator()
	    {
		    return m_Bookings.GetEnumerator();
	    }

	    /// <summary>Returns an enumerator that iterates through a collection.</summary>
	    /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return GetEnumerator();
	    }

	    #endregion
    }
}
