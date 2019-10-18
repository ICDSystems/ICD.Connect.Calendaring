using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Robin.Components.Events;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar
{
	public sealed class RobinBooking : AbstractBooking
	{
		private readonly Event m_Event;
		private readonly List<IDialContext> m_DialContexts;

		public Event Event { get { return m_Event; } }

		public override string MeetingName
		{
			get { return m_Event.MeetingName; }
		}

		public override string OrganizerName
		{
			get { return m_Event.OrganizerName; }
		}

		public override string OrganizerEmail
		{
			get { return m_Event.OrganizerEmail; }
		}

		public override DateTime StartTime
		{
			get { return m_Event.MeetingStart.DateTimeInfo; }
		}

		public override DateTime EndTime
		{
			get { return m_Event.MeetingEnd.DateTimeInfo; }
		}

		public override bool IsPrivate
		{
			get { return m_Event.IsPrivate.ToLower() == "private"; }
		}

		public override bool CheckedIn { get { return false; } }
		public override bool CheckedOut { get { return false; } }

		public override IEnumerable<IDialContext> GetBookingNumbers()
		{
			return m_DialContexts.ToArray(m_DialContexts.Count);
		}

		public RobinBooking(Event robinEvent, IEnumerable<IDialContext> dialContexts)
		{
			if (robinEvent == null)
				throw new ArgumentNullException("robinEvent");

			m_Event = robinEvent;

			if (dialContexts != null)
				m_DialContexts = dialContexts.ToList();
		}
	}
}