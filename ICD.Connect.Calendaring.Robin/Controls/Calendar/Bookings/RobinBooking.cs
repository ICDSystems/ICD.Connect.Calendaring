using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Robin.Components.Events;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar.Bookings
{
	public sealed class RobinBooking : AbstractBooking
	{
		private readonly Event m_Event;
		private readonly List<IDialContext> m_BookingNumbers;

		public override string MeetingName
		{
			get { return m_Event.MeetingName; }
		}

		public override string OrganizerName
		{
			get { return m_Event.OrganizerName; }
		}

		public override string OrganizerEmail
		{
			get { return m_Event.OrganizerEmail; }
		}

		public override DateTime StartTime
		{
			get { return m_Event.MeetingStart.DateTimeInfo; }
		}

		public override DateTime EndTime
		{
			get { return m_Event.MeetingEnd.DateTimeInfo; }
		}

		public override bool IsPrivate
		{
			get { return m_Event.IsPrivate.ToLower() == "private"; }
		}

		public override IEnumerable<IDialContext> GetBookingNumbers()
		{
			return m_BookingNumbers.ToArray(m_BookingNumbers.Count);
		}

		public RobinBooking(Event @event, IEnumerable<BookingProtocolInfo> bookingProtocolInfo)
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