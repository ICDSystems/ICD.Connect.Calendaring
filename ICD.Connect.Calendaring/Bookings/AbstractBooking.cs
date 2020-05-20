using System;
using System.Collections.Generic;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Bookings
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
		/// Returns true if the booking is checked in.
		/// </summary>
		public abstract bool CheckedIn { get; }

		/// <summary>
		/// Returns true if the booking is checked out.
		/// </summary>
		public abstract bool CheckedOut { get; }

		/// <summary>
		/// Returns Booking Numbers.
		/// </summary>
		public abstract IEnumerable<IDialContext> GetBookingNumbers();
	}
}