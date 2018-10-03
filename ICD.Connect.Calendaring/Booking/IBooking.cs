using System;
using System.Collections.Generic;

namespace ICD.Connect.Calendaring.Booking
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
		/// Returns the meeting type.
		/// </summary>
		eMeetingType Type { get; }

		/// <summary>
		/// Returns true if meeting is private.
		/// </summary>
		bool IsPrivate { get; }

		/// <summary>
		/// Returns Booking Numbers.
		/// </summary>
		IEnumerable<IBookingNumber> GetBookingNumbers();
	}
}
