using System;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Robin.Components.Events;

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar.Bookings
{
	public abstract class AbstractRobinBooking : IRobinBooking
	{
		private readonly Event m_Event;

		public virtual string MeetingName
		{
			get { return m_Event.MeetingName; }
		}

		public virtual string OrganizerName
		{
			get { return m_Event.OrganizerName; }
		}

		public virtual string OrganizerEmail
		{
			get { return m_Event.OrganizerEmail; }
		}

		public virtual DateTime StartTime
		{
			get { return m_Event.MeetingStart.DateTimeInfo; }
		}

		public virtual DateTime EndTime
		{
			get { return m_Event.MeetingEnd.DateTimeInfo; }
		}

		public virtual bool IsPrivate
		{
			get { return m_Event.IsPrivate.ToLower() == "private"; }
		}

		public virtual eMeetingType Type
		{
			get { return eMeetingType.VideoConference; }
		}

		public virtual string Description
		{
			get { return m_Event.Description; }
		}

		public AbstractRobinBooking(Event @event)
		{
			m_Event = @event;
		}
	}
}