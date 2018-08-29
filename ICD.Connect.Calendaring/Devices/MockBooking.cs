using System;
using ICD.Connect.Calendaring.Booking;

namespace ICD.Connect.Calendaring.Devices
{
	public class MockBooking : AbstractBooking
	{
		public override string MeetingName { get;}
		public override string OrganizerName { get; }
		public override string OrganizerEmail { get; }
		public override DateTime StartTime { get; }
		public override DateTime EndTime { get; }
		public override eMeetingType Type { get; }

		public MockBooking(string meetingName, string organizerName, string organizerEmail, DateTime startTime, DateTime endTime)
		{
			MeetingName = meetingName;
			OrganizerName = organizerName;
			OrganizerEmail = organizerEmail;
			StartTime = startTime;
			EndTime = endTime;
			Type = eMeetingType.AudioConference;
		}
	}
}