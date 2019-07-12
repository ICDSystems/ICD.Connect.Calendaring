using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Devices.iCalendar.Parser;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.iCalendar.Controls.Calendar
{
// ReSharper disable InconsistentNaming
	public sealed class iCalendarBooking : AbstractBooking
// ReSharper restore InconsistentNaming
	{
		private readonly iCalendarEvent m_Event;
		private readonly IList<IDialContext> m_BookingNumbers;

		public iCalendarEvent Event { get { return m_Event; } }

		public override string MeetingName
		{
			get { return m_Event.Summary; }
		}

		public override string OrganizerName
		{
			get { return m_Event.Organizer == null ? null : m_Event.Organizer.Cn; }
		}

		public override string OrganizerEmail
		{
			get
			{
				if (m_Event.Organizer == null)
					return null;

				return m_Event.Organizer.Mailto ?? m_Event.Organizer.Cn;
			}
		}

		public override DateTime StartTime
		{
			get { return m_Event.DtStart.ToLocalTime(); }
		}

		public override DateTime EndTime
		{
			get { return m_Event.DtEnd.ToLocalTime(); }
		}

		public override bool IsPrivate
		{
			get { return m_Event.Class != "PUBLIC"; }  
		}

		public override IEnumerable<IDialContext> GetBookingNumbers()
		{
			return m_BookingNumbers.ToArray(m_BookingNumbers.Count);
		}

		public iCalendarBooking(iCalendarEvent @event, IEnumerable<BookingProtocolInfo> bookingProtocolInfo)
		{
			if (@event == null)
				throw new ArgumentNullException("event");

			m_Event = @event;

			if (bookingProtocolInfo != null)
				m_BookingNumbers = ParseBookingNumbers(bookingProtocolInfo).ToList();
		}

		private static IEnumerable<IDialContext> ParseBookingNumbers(IEnumerable<BookingProtocolInfo> bookingProtocolInfo)
		{
			foreach (BookingProtocolInfo info in bookingProtocolInfo)
			{
				switch (info.DialProtocol)
				{
					case eDialProtocol.None:
						continue;

					case eDialProtocol.Sip:
						yield return new SipDialContext { DialString = info.Number };
						continue;

					case eDialProtocol.Pstn:
						yield return new PstnDialContext { DialString = info.Number };
						continue;

					case eDialProtocol.Zoom:
						yield return new ZoomDialContext { DialString = info.Number };
						continue;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}