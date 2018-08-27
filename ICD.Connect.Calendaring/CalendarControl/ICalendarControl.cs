using System;
using System.Collections.Generic;
using ICD.Connect.Calendering.Booking;

namespace ICD.Connect.Calendering.CalendarControl
{
	public interface ICalendarControl
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
