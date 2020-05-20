using System;
using System.Collections.Generic;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Bookings
{
	public interface IBooking
	{
		/// <summary>
		/// Returns the name of the meeting.
		/// </summary>
		string MeetingName { get; }

        /// <summary>
        /// Returns the organizer's name.
        /// </summary>
        string OrganizerName { get; }

		/// <summary>
		/// Returns the organizer's email.
		/// </summary>
		string OrganizerEmail { get; }

		/// <summary>
		/// Returns the meeting start time.
		/// </summary>
		DateTime StartTime { get; }

		/// <summary>
		/// Returns the meeting end time.
		/// </summary>
		DateTime EndTime { get; }

		/// <summary>
		/// Returns true if meeting is private.
		/// </summary>
		bool IsPrivate { get; }

		/// <summary>
		/// Returns true if the booking is checked in.
		/// </summary>
		bool CheckedIn { get; }

		/// <summary>
		/// Returns true if the booking is checked out.
		/// </summary>
		bool CheckedOut { get; }

		/// <summary>
		/// Returns Booking Numbers.
		/// </summary>
		IEnumerable<IDialContext> GetBookingNumbers();
	}
}
