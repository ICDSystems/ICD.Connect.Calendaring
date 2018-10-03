namespace ICD.Connect.Calendaring
{
	public sealed class BookingProtocolInfo
    {
	    public eBookingProtocol BookingProtocol { get; set; }
		public string Number { get; set; }
    }

	/// <summary>
	/// The types of protocols for booking numbers.
	/// Arranged in ascending order of least qualified to most qualified.
	/// </summary>
	public enum eBookingProtocol
	{
		None = 0,
		Pstn = 1,
		Sip = 2,
		Zoom = 3
	}
}
