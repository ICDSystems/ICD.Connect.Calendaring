using System;
using System.Collections.Generic;
using ICD.Connect.Calendaring.Booking;

namespace ICD.Connect.Calendaring.Devices
{
	public class MockBooking : AbstractBooking
	{
		private readonly string m_MeetingName;
        private readonly string m_OrganizerName;
		private readonly string m_OrganizerEmail;
		private readonly DateTime m_StartTime;
		private readonly DateTime m_EndTime;
		private readonly bool m_IsPrivate;

		public override string MeetingName
		{
			get { return m_MeetingName; }
		}

        public override string OrganizerName
		{
			get { return m_OrganizerName; }
		}
		public override string OrganizerEmail
		{
			get { return m_OrganizerEmail; }
		}
		public override DateTime StartTime
		{
			get { return m_StartTime; }
		}
		public override DateTime EndTime
		{
			get { return m_EndTime; }
		}
		public override bool IsPrivate
		{
			get { return m_IsPrivate; }
		}

		public override IEnumerable<IBookingNumber> GetBookingNumbers()
		{
			throw new NotImplementedException();
		}

		public override eMeetingType Type
		{
			get { return eMeetingType.AudioConference; }
		}

		public MockBooking(string meetingName, string organizerName, string organizerEmail, DateTime startTime, DateTime endTime, bool isPrivate)
		{
			m_MeetingName = meetingName;
            m_OrganizerName = organizerName;
			m_OrganizerEmail = organizerEmail;
			m_StartTime = startTime;
			m_EndTime = endTime;
			m_IsPrivate = isPrivate;
		}
	}
}