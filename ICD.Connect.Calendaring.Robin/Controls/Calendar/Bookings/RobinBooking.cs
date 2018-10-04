using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Robin.Components.Events;

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

	    public RobinBooking(Event @event, IEnumerable<BookingProtocolInfo> bookingProtocolInfo)
	    {
	        if (@event == null)
	            throw new ArgumentNullException("event");

	        m_Event = @event;

	        if (bookingProtocolInfo != null)
                m_BookingNumbers = ParseBookingNumbers(bookingProtocolInfo).ToList();
	    }

		private static IEnumerable<IBookingNumber> ParseBookingNumbers(IEnumerable<BookingProtocolInfo> bookingProtocolInfo)
	    {
	        foreach (BookingProtocolInfo info in bookingProtocolInfo)
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