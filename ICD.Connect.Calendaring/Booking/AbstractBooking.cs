using System;

namespace ICD.Connect.Calendaring_NetStandard
{
	public abstract class AbstractBooking : IConferenceBooking
	{
		private readonly string m_MeetingName;
		private readonly string m_OrganizerName;
		private readonly string m_OrganizerEmail;
		private readonly DateTime m_StartTime;
		private readonly DateTime m_EndTime;
		private readonly eMeetingType m_Type;

		public string MeetingName { get { return m_MeetingName; } }
		public string OrganizerName { get { return m_OrganizerName; } }
		public string OrganizerEmail { get { return m_OrganizerEmail; } }
		public DateTime StartTime { get { return m_StartTime; } }
		public DateTime EndTime { get { return m_EndTime; } }
		public eMeetingType Type { get { return m_Type; } }
	}
}