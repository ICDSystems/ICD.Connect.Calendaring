using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Robin.Components.Bookings;

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar.Bookings
{
	public sealed class RobinBooking : AbstractBooking
	{
		private readonly Event m_Event;
		private readonly List<IBookingNumber> m_BookingNumbers;

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

		public override IEnumerable<IBookingNumber> GetBookingNumbers()
		{
			return m_BookingNumbers.ToArray(m_BookingNumbers.Count);
		}

		public override eMeetingType Type
		{
			get { return eMeetingType.VideoConference; }
		}

		public RobinBooking(Event @event)
		{
			if (@event == null)
				throw new ArgumentNullException("event");

			m_Event = @event;
			m_BookingNumbers = ParseBookingNumbers(m_Event.Description).ToList();
		}

		private static IEnumerable<IBookingNumber> ParseBookingNumbers(string eventDescription)
		{
			IEnumerable<BookingProtocolInfo> infos = BookingParsingUtils.GetProtocolInfos(eventDescription);

			foreach (BookingProtocolInfo info in infos)
			{
				switch (info.BookingProtocol)
				{
					case eBookingProtocol.None:
						continue;

					case eBookingProtocol.Sip:
						yield return new SipBookingNumber(info);
						continue;

					case eBookingProtocol.Pstn:
						yield return new PstnBookingNumber(info);
						continue;

					case eBookingProtocol.Zoom:
						yield return new ZoomBookingNumber(info);
						continue;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}