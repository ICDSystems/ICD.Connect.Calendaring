#if !SIMPLSHARP
extern alias RealIndependentsoft;
using RealIndependentsoft.Independentsoft.Exchange;
#else
using Independentsoft.Exchange;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Email;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Microsoft.Exchange.Controls
{
	public sealed class ExchangeBooking : AbstractBooking
	{
		private readonly Appointment m_CalendarEvent;
		private readonly IList<IDialContext> m_BookingNumbers;

		/// <summary>
		/// Returns the name of the meeting.
		/// </summary>
		public override string MeetingName { get { return m_CalendarEvent.Subject; } }

		/// <summary>
		/// Returns the organizer's name.
		/// </summary>
		public override string OrganizerName { get { return m_CalendarEvent.Organizer.Name; } }

		/// <summary>
		/// Returns the organizer's email.
		/// </summary>
		public override string OrganizerEmail
		{
			get
			{
				return EmailValidation.IsValidEmailAddress(m_CalendarEvent.Organizer.EmailAddress)
					       ? m_CalendarEvent.Organizer.EmailAddress
					       : null;
			}
		}

		/// <summary>
		/// Returns the meeting start time.
		/// </summary>
		public override DateTime StartTime { get { return m_CalendarEvent.StartTime.ToLocalTime(); } }

		/// <summary>
		/// Returns the meeting end time.
		/// </summary>
		public override DateTime EndTime { get { return m_CalendarEvent.EndTime.ToLocalTime(); } }

		/// <summary>
		/// Returns true if meeting is private.
		/// </summary>
		public override bool IsPrivate { get { return false; } }

		/// <summary>
		/// Returns true if the booking is checked in.
		/// </summary>
		public override bool CheckedIn { get { return false; } }

		/// <summary>
		/// Returns true if the booking is checked out.
		/// </summary>
		public override bool CheckedOut { get { return false; } }

		public override IEnumerable<IDialContext> GetBookingNumbers()
		{
			return m_BookingNumbers.ToArray(m_BookingNumbers.Count);
		}

		public ExchangeBooking(Appointment calendarEvent, IEnumerable<IDialContext> dialContexts)
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
