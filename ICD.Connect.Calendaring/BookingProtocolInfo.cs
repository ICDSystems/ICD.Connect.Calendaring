namespace ICD.Connect.Calendaring
{
	public sealed class BookingProtocolInfo
    {
	    public eBookingProtocol BookingProtocol { get; set; }
		public string Number { get; set; }
    }

	public enum eBookingProtocol
	{
		None,
		Sip,
		Pstn,
		Zoom
	}
}
