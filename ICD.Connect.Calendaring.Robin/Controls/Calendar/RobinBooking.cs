using System;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Robin.Components.Bookings;

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar
{
    public sealed class RobinBooking : AbstractBooking
    {
	    private readonly Event m_Event;

	    public override string MeetingName
	    {
		    get { return m_Event.MeetingName; }
		}

		public override string OrganizerName
	    {
		    get { return m_Event.OrganizerName;  }
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

		public override eMeetingType Type
	    {
		    get { return eMeetingType.VideoConference; }
	    }

	    public RobinBooking(Event @event)
	    {
		    m_Event = @event;
	    }
    }
}
