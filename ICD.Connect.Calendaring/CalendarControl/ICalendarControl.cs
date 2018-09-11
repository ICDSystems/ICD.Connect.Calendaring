using System;
using System.Collections.Generic;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Calendaring.CalendarControl
{
	public interface ICalendarControl : IDeviceControl
	{
		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		event EventHandler OnBookingsChanged;

		/// <summary>
		/// Updates the view.
		/// </summary>
		/// <param name="command"></param>
		void Refresh();

		/// <summary>
		/// Gets booking information.
		/// </summary>
		/// <param name="command"></param>
		IEnumerable<IBooking> GetBookings();
	}
}
