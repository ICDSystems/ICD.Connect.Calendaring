using System;

namespace ICD.Connect.Calendaring.Booking
{
	public abstract class AbstractBooking : IBooking
	{
		public abstract string MeetingName { get; }
		public abstract string OrganizerName { get; }
		public abstract string OrganizerEmail { get; }
		public abstract DateTime StartTime { get; }
		public abstract DateTime EndTime { get; }
		public abstract eMeetingType Type { get; }
	}
}