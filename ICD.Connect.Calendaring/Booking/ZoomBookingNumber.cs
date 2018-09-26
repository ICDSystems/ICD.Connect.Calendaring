namespace ICD.Connect.Calendaring.Booking
{
	public sealed class ZoomBookingNumber : AbstractBookingNumber, IZoomBookingNumber
	{
		public ZoomBookingNumber(BookingProtocolInfo info)
			: this(info.Number)
		{
			MeetingNumber = info.Number;
		}

		public ZoomBookingNumber(string number)
		{
			MeetingNumber = number;
		}

		public string MeetingNumber { get; }
		public override eBookingProtocol Protocol { get { return eBookingProtocol.Zoom; } }
		public string SipUri
		{
			get { return "sip:" + MeetingNumber + "@zmus.us"; }
		}
	}
}