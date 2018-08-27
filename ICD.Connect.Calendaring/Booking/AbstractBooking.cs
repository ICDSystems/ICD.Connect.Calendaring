using System;
using System.Collections.Generic;
using ICD.Common.Properties;

namespace ICD.Connect.Calendaring_NetStandard.Booking
{
	public abstract class AbstractBooking : IConferenceBooking
	{
		public abstract string MeetingName { get; }
		public abstract string OrganizerName { get; }
		public abstract string OrganizerEmail { get; }
		public abstract DateTime StartTime { get; }
		public abstract DateTime EndTime { get; }
		public virtual eMeetingType Type { get; }
	}
}