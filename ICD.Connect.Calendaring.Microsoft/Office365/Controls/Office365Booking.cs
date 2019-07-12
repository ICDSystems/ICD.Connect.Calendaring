using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Controls
{
	public sealed class Office365Booking : AbstractBooking
	{
		private readonly CalendarEvent m_CalendarEvent;
		private readonly IList<IDialContext> m_BookingNumbers;

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
		/// Returns Booking Numbers.
		/// </summary>
		public override IEnumerable<IDialContext> GetBookingNumbers()
		{
			return m_BookingNumbers.ToArray(m_BookingNumbers.Count);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="calendarEvent"></param>
		/// <param name="dialContexts"></param>
		public Office365Booking(CalendarEvent calendarEvent, IEnumerable<IDialContext> dialContexts)
		{
			if (calendarEvent == null)
				throw new ArgumentNullException("calendarEvent");

			if (dialContexts == null)
				throw new ArgumentNullException("dialContexts");

			m_CalendarEvent = calendarEvent;

			m_BookingNumbers = dialContexts.ToList();
		}
	}
}