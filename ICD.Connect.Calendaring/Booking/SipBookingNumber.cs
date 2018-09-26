using System;

namespace ICD.Connect.Calendaring.Booking
{
	public sealed class SipBookingNumber : AbstractBookingNumber, ISipBookingNumber
	{
		public SipBookingNumber(BookingProtocolInfo info) 
			: this(info.Number)
		{
		}

		public SipBookingNumber(string number)
		{
			SipUri = number;
		}

		public string SipUri { get; }
		public override eBookingProtocol Protocol { get {return eBookingProtocol.Sip;} }
	}
}