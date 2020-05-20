using System;
using System.Collections.Generic;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Devices.Mock
{
	public sealed class MockBooking : AbstractBooking
	{
		private readonly string m_MeetingName;
        private readonly string m_OrganizerName;
		private readonly string m_OrganizerEmail;
		private readonly DateTime m_StartTime;
		private readonly DateTime m_EndTime;
		private readonly bool m_IsPrivate;
		private readonly bool m_CheckedIn;
		private readonly bool m_CheckedOut;

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

		public override bool CheckedIn
		{
			get { return m_CheckedIn; }
		}

		public override bool CheckedOut
		{
			get { return m_CheckedOut; }
		}

		public override IEnumerable<IDialContext> GetBookingNumbers()
		{
			return new List<IDialContext>{ new PstnDialContext { DialString = "5555555555" } };
		}

		public MockBooking(string meetingName, string organizerName, string organizerEmail, DateTime startTime, DateTime endTime, bool isPrivate, bool checkedIn, bool checkedOut)
		{
			m_MeetingName = meetingName;
            m_OrganizerName = organizerName;
			m_OrganizerEmail = organizerEmail;
			m_StartTime = startTime;
			m_EndTime = endTime;
			m_IsPrivate = isPrivate;
			m_CheckedIn = checkedIn;
			m_CheckedOut = checkedOut;
		}
	}
}