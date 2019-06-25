using System;
using System.Collections.Generic;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.CalendarControl;

namespace ICD.Connect.Calendaring.Microsoft.Office365
{
	public sealed class Office365CalendarDeviceCalendarControl : AbstractCalendarControl<Office365CalendarDevice>
	{
		public override event EventHandler OnBookingsChanged;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public Office365CalendarDeviceCalendarControl(Office365CalendarDevice parent, int id)
			: base(parent, id)
		{
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override void Refresh()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override IEnumerable<IBooking> GetBookings()
		{
			throw new NotImplementedException();
		}
	}
}