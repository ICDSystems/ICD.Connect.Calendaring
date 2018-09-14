using System;
using ICD.Connect.Calendaring.Booking;

namespace ICD.Connect.Calendaring.Robin.Controls.Calendar
{
    public sealed class RobinBooking : AbstractBooking
    {
	    private readonly Components.Bookings.Booking m_Booking;

	    public override string MeetingName
	    {
		    get { return m_Booking.MeetingName; }
		}

		public override string OrganizerName
	    {
		    get { return m_Booking.OrganizerName;  }
	    }

	    public override string OrganizerEmail
	    {
			get { return m_Booking.OrganizerEmail; }
		}

	    public override DateTime StartTime
	    {
			get { return m_Booking.MeetingStart.DateTimeInfo; }
		}

	    public override DateTime EndTime
	    {
			get { return m_Booking.MeetingEnd.DateTimeInfo; }
		}

	    public override bool IsPrivate
	    {
		    get { return m_Booking.IsPrivate.ToLower() == "private"; }
	    }

		public override eMeetingType Type
	    {
		    get { return eMeetingType.VideoConference; }
	    }

	    public RobinBooking(Components.Bookings.Booking booking)
	    {
		    m_Booking = booking;
	    }
    }
}
