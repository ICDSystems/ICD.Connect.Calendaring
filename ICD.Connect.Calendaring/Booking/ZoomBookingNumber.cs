namespace ICD.Connect.Calendaring.Booking
{
	public sealed class ZoomBookingNumber : AbstractBookingNumber, IZoomBookingNumber
	{
		private readonly string m_MeetingNumber;
		
		public ZoomBookingNumber(BookingProtocolInfo info)
			: this(info.Number)
		{
		}

		public ZoomBookingNumber(string number)
		{
			m_MeetingNumber = number;
		}

		public string MeetingNumber
		{
			get { return m_MeetingNumber; }
		}

		public override eBookingProtocol Protocol
		{
			get { return eBookingProtocol.Zoom; }
		}

		public string SipUri
		{
			get { return "sip:" + MeetingNumber + "@zmus.us"; }
		}

		public override string ToString()
		{
			return MeetingNumber + "@zmus.us";
		}
	}
}