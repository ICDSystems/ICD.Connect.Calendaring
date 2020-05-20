using System;
using System.Collections.Generic;
using ICD.Connect.Calendaring.Bookings;
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

		/// <summary>
		/// Returns true if the booking argument can be checked in.
		/// </summary>
		/// <returns></returns>
		bool CanCheckIn(IBooking booking);

		/// <summary>
		/// Returns true if the booking argument can be checked out of.
		/// </summary>
		/// <param name="booking"></param>
		/// <returns></returns>
		bool CanCheckOut(IBooking booking);

		/// <summary>
		/// Checks in to the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		void CheckIn(IBooking booking);

		/// <summary>
		/// Checks out of the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		void CheckOut(IBooking booking);
	}
}
