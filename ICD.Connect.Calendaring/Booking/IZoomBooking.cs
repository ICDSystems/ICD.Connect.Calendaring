namespace ICD.Connect.Calendaring.Booking
{
	public interface IZoomBooking : ISipBooking
	{
		string MeetingNumber { get; }
	}
}