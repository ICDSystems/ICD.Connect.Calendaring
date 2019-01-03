using System;
using System.Collections.Generic;

namespace ICD.Connect.Calendaring.Booking
{
	public abstract class AbstractBooking : IBooking
	{
		/// <summary>
		/// Returns the name of the meeting.
		/// </summary>
		public abstract string MeetingName { get; }

		/// <summary>
		/// Returns the organizer's name.
		/// </summary>
		public abstract string OrganizerName { get; }

		/// <summary>
		/// Returns the organizer's email.
		/// </summary>
		public abstract string OrganizerEmail { get; }

		/// <summary>
		/// Returns the meeting start time.
		/// </summary>
		public abstract DateTime StartTime { get; }

		/// <summary>
		/// Returns the meeting end time.
		/// </summary>
		public abstract DateTime EndTime { get; }

		/// <summary>
		/// Returns true if meeting is private.
		/// </summary>
		public abstract bool IsPrivate { get; }

		/// <summary>
		/// Returns Booking Numbers.
		/// </summary>
		public abstract IEnumerable<IBookingNumber> GetBookingNumbers();
	}
}