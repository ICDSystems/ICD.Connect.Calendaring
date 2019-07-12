using System;
using System.Collections.Generic;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Calendaring.Controls
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
		void Refresh();

		/// <summary>
		/// Gets the collection of calendar bookings.
		/// </summary>
		IEnumerable<IBooking> GetBookings();
	}
}
