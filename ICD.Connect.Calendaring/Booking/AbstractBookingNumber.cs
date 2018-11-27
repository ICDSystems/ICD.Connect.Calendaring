namespace ICD.Connect.Calendaring.Booking
{
    public abstract class AbstractBookingNumber : IBookingNumber
    {
	    public abstract eBookingProtocol Protocol { get; }
    }
}
