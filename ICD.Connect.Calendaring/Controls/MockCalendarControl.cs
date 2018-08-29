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

		public MockCalendarControl(MockCalendarDevice parent, int id) : base(parent, id)
		{
			m_BookingList = new List<MockBooking>();
		}

		public override event EventHandler OnBookingsChanged;

		public override void Refresh()
		{
			m_BookingList.Add(new MockBooking("Meeting1", "Organizer1@email.biz", "The Organizer1", new DateTime(2018, 8, 27, 12, 30, 0), new DateTime(2018, 8, 27, 13, 0, 0)));
			m_BookingList.Add(new MockBooking("Meeting2", "Organizer2@email.biz", "The Organizer2", new DateTime(2018, 8, 27, 11, 30, 0), new DateTime(2018, 8, 27, 12, 0, 0)));
			m_BookingList.Add(new MockBooking("Meeting3", "Organizer3@email.biz", "The Organizer3", new DateTime(2018, 8, 27, 10, 30, 0), new DateTime(2018, 8, 27, 11, 0, 0)));
			m_BookingList.Add(new MockBooking("Meeting4", "Organizer4@email.biz", "The Organizer4", new DateTime(2018, 8, 27, 10, 0, 0), new DateTime(2018, 8, 27, 10, 30, 0)));
			m_BookingList.Add(new MockBooking("Meeting5", "Organizer5@email.biz", "The Organizer5", new DateTime(2018, 8, 27, 13, 30, 0), new DateTime(2018, 8, 27, 14, 0, 0)));

			foreach (var booking in m_BookingList)
			{
				IcdConsole.PrintLine(booking.ToString());
			}
		}

		public override IEnumerable<IBooking> GetBookings()
		{
			m_BookingList.Add(new MockBooking("Meeting1", "Organizer1@email.biz", "The Organizer1", new DateTime(2018, 8, 12, 12, 30, 0), new DateTime(2018, 8, 12, 13, 0, 0)));
			m_BookingList.Add(new MockBooking("Meeting2", "Organizer2@email.biz", "The Organizer2", new DateTime(2018, 8, 12, 11, 30, 0), new DateTime(2018, 8, 12, 12, 0, 0)));
			m_BookingList.Add(new MockBooking("Meeting3", "Organizer3@email.biz", "The Organizer3", new DateTime(2018, 8, 12, 10, 30, 0), new DateTime(2018, 8, 12, 11, 0, 0)));
			m_BookingList.Add(new MockBooking("Meeting4", "Organizer4@email.biz", "The Organizer4", new DateTime(2018, 8, 12, 10, 0, 0), new DateTime(2018, 8, 12, 10, 30, 0)));
			m_BookingList.Add(new MockBooking("Meeting5", "Organizer5@email.biz", "The Organizer5", new DateTime(2018, 8, 12, 13, 30, 0), new DateTime(2018, 8, 12, 14, 0, 0)));

			return m_BookingList;
		}
	}
}