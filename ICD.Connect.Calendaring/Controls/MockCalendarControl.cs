using System;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.CalendarControl;
using ICD.Connect.Calendaring.Devices;

namespace ICD.Connect.Calendaring.Controls
{
	public sealed class MockCalendarControl : AbstractCalendarControl<MockCalendarDevice>
	{
		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		public override event EventHandler OnBookingsChanged;

		private readonly List<IBooking> m_BookingList;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public MockCalendarControl(MockCalendarDevice parent, int id)
			: base(parent, id)
		{
			m_BookingList = new List<IBooking>();
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override void Refresh()
		{
			m_BookingList.Clear();

			DateTime timeNow = IcdEnvironment.GetLocalTime();
			DateTime defaultMeetingTime = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, 0, 0);

			m_BookingList.Add(new MockBooking("Meeting1", "1", "Organizer1@email.biz", "The Organizer1", defaultMeetingTime.AddHours(-3), defaultMeetingTime.AddHours(-3).AddMinutes(30), true));
			m_BookingList.Add(new MockBooking("Meeting2", "2", "Organizer2@email.biz", "The Organizer2", defaultMeetingTime.AddHours(-2).AddMinutes(30), defaultMeetingTime.AddHours(-1), false));
			m_BookingList.Add(new MockBooking("Meeting4", "4", "Organizer4@email.biz", "The Organizer4", defaultMeetingTime, defaultMeetingTime.AddMinutes(30), false));
			m_BookingList.Add(new MockBooking("Meeting3", "3", "Organizer3@email.biz", "The Organizer3", defaultMeetingTime.AddHours(1), defaultMeetingTime.AddHours(1).AddMinutes(30), true));
			m_BookingList.Add(new MockBooking("Meeting5", "5", "Organizer5@email.biz", "The Organizer5", defaultMeetingTime.AddHours(2), defaultMeetingTime.AddHours(2).AddMinutes(30), false));

			OnBookingsChanged.Raise(this);
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override IEnumerable<IBooking> GetBookings()
		{
			return m_BookingList;
		}
	}
}