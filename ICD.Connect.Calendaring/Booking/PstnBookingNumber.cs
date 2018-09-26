namespace ICD.Connect.Calendaring.Booking
{
	public sealed class PstnBookingNumber : AbstractBookingNumber, IPstnBookingNumber
	{
		public PstnBookingNumber(BookingProtocolInfo info)
		{
			PhoneNumber = info.Number;
			Protocol = info.BookingProtocol;
		}

		public string PhoneNumber { get; }
		public override eBookingProtocol Protocol { get; }
	}
}