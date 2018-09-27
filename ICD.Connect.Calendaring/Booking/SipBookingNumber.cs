using System;

namespace ICD.Connect.Calendaring.Booking
{
	public sealed class SipBookingNumber : AbstractBookingNumber, ISipBookingNumber
	{
		private readonly string m_SipUri;

		public SipBookingNumber(BookingProtocolInfo info) 
			: this(info.Number)
		{
		}

		public SipBookingNumber(string number)
		{
			m_SipUri = number;
		}

		public string SipUri
		{
			get { return m_SipUri; }
		}

		public override eBookingProtocol Protocol
		{
			get {return eBookingProtocol.Sip;}
		}
	}
}