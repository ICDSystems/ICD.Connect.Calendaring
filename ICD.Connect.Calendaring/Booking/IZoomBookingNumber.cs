namespace ICD.Connect.Calendaring.Booking
{
	public interface IZoomBookingNumber : ISipBookingNumber
	{
		string MeetingNumber { get; }
	}
}