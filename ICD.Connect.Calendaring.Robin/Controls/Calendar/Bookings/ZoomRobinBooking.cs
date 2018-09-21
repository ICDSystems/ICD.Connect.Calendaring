using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Robin.Components.Bookings;

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar.Bookings
{
	public sealed class ZoomRobinBooking : AbstractRobinBooking, IZoomBooking
	{

		public string MeetingNumber { get; set; }

		public string SipUri
		{
			get { return MeetingNumber == null ? null : "sip:" + MeetingNumber + "@zmus.us"; }
		}

		public ZoomRobinBooking(Event @event) : base(@event)
		{
		}
	}
}
