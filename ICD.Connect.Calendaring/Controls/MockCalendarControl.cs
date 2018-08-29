using System;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.CalendarControl;
using ICD.Connect.Calendaring.Devices;

namespace ICD.Connect.Calendaring.Controls
{
	public class MockCalendarControl : AbstractCalendarControl<MockCalendarDevice>
	{

		private readonly List<MockBooking> m_BookingList;
		private readonly DateTime m_DefaultMeetingTime;

		public MockCalendarControl(MockCalendarDevice parent, int id) : base(parent, id)
		{
			DateTime timeNow = IcdEnvironment.GetLocalTime();
			m_BookingList = new List<MockBooking>();
			m_DefaultMeetingTime = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, 0, 0);
		}

		public override event EventHandler OnBookingsChanged;

		public override void Refresh()
		{
			m_BookingList.Clear();

			m_BookingList.Add(new MockBooking("Meeting1", "Organizer1@email.biz", "The Organizer1", m_DefaultMeetingTime.AddHours(-3), m_DefaultMeetingTime.AddHours(-3).AddMinutes(30)));
			m_BookingList.Add(new MockBooking("Meeting2", "Organizer2@email.biz", "The Organizer2", m_DefaultMeetingTime.AddHours(-2).AddMinutes(30), m_DefaultMeetingTime.AddHours(-1)));
			m_BookingList.Add(new MockBooking("Meeting3", "Organizer3@email.biz", "The Organizer3", m_DefaultMeetingTime.AddHours(-1), m_DefaultMeetingTime.AddHours(-1).AddMinutes(30)));
			m_BookingList.Add(new MockBooking("Meeting4", "Organizer4@email.biz", "The Organizer4", m_DefaultMeetingTime, m_DefaultMeetingTime.AddMinutes(30)));
			m_BookingList.Add(new MockBooking("Meeting5", "Organizer5@email.biz", "The Organizer5", m_DefaultMeetingTime.AddHours(1), m_DefaultMeetingTime.AddMinutes(30)));
		}

		public override IEnumerable<IBooking> GetBookings()
		{
			return m_BookingList;
		}
	}
}