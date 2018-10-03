namespace ICD.Connect.Calendaring.Booking
{
	public sealed class PstnBookingNumber : AbstractBookingNumber, IPstnBookingNumber
	{
		private readonly string m_PhoneNumber;

		public PstnBookingNumber(BookingProtocolInfo info)
		{
			m_PhoneNumber = info.Number;
		}

		public string PhoneNumber
		{
			get { return m_PhoneNumber; }
		}

		public override eBookingProtocol Protocol 
		{
			get { return eBookingProtocol.Pstn; }
		}
	}
}