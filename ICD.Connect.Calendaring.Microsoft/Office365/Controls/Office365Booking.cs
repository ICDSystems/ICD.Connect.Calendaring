using System;
using System.Collections.Generic;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Controls
{
	public sealed class Office365Booking : AbstractBooking
	{
		private readonly CalendarEvent m_CalendarEvent;

		/// <summary>
		/// Returns the name of the meeting.
		/// </summary>
		public override string MeetingName { get { return m_CalendarEvent.Subject; } }

		/// <summary>
		/// Returns the organizer's name.
		/// </summary>
		public override string OrganizerName { get { return m_CalendarEvent.Organizer.EmailAddress.Name; } }

		/// <summary>
		/// Returns the organizer's email.
		/// </summary>
		public override string OrganizerEmail { get { return m_CalendarEvent.Organizer.EmailAddress.Address; } }

		/// <summary>
		/// Returns the meeting start time.
		/// </summary>
		public override DateTime StartTime { get { return m_CalendarEvent.Start.DateTime; } }

		/// <summary>
		/// Returns the meeting end time.
		/// </summary>
		public override DateTime EndTime { get { return m_CalendarEvent.End.DateTime; } }

		/// <summary>
		/// Returns true if meeting is private.
		/// </summary>
		public override bool IsPrivate { get { return false; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="calendarEvent"></param>
		public Office365Booking(CalendarEvent calendarEvent)
		{
			if (calendarEvent == null)
				throw new ArgumentNullException("event");

			m_CalendarEvent = calendarEvent;
		}

		/// <summary>
		/// Returns Booking Numbers.
		/// </summary>
		public override IEnumerable<IDialContext> GetBookingNumbers()
		{
			yield break;
		}
	}
}