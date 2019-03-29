﻿using System;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Timers;
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

		private const int TIMER_REFRESH_INTERVAL = 10 * 60 * 1000;

		private readonly List<IBooking> m_BookingList;
		private readonly SafeTimer m_RefreshTimer;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public MockCalendarControl(MockCalendarDevice parent, int id)
			: base(parent, id)
		{
			m_BookingList = new List<IBooking>();
			m_RefreshTimer = new SafeTimer(Refresh, TIMER_REFRESH_INTERVAL, TIMER_REFRESH_INTERVAL);
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnBookingsChanged = null;

			m_RefreshTimer.Dispose();

			base.DisposeFinal(disposing);
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override void Refresh()
		{
			m_BookingList.Clear();

			DateTime timeNow = IcdEnvironment.GetLocalTime();
			DateTime defaultMeetingTime = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, 0, 0);

			m_BookingList.Add(new MockBooking("Old Meeting 1", "The Organizer1", "Organizer1@email.biz", defaultMeetingTime.AddHours(-3), defaultMeetingTime.AddHours(-3).AddMinutes(30), true));
			m_BookingList.Add(new MockBooking("Old Meeting 2", "The Organizer2", "Organizer2@email.biz", defaultMeetingTime.AddHours(-2).AddMinutes(30), defaultMeetingTime.AddHours(-1), false));
			m_BookingList.Add(new MockBooking("New Meeting 1", "The Organizer3", "Organizer3@email.biz", defaultMeetingTime, defaultMeetingTime.AddMinutes(30), false));
			m_BookingList.Add(new MockBooking("New Meeting 2", "The Organizer4", "Organizer4@email.biz", defaultMeetingTime.AddHours(1), defaultMeetingTime.AddHours(1).AddMinutes(30), true));
			m_BookingList.Add(new MockBooking("New Meeting 3", "The Organizer5", "Organizer5@email.biz", defaultMeetingTime.AddHours(2), defaultMeetingTime.AddHours(2).AddMinutes(30), false));

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