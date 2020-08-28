using System;
using System.Collections.Generic;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Calendaring.Controls
{
	public interface ICalendarControl : IDeviceControl
	{
		#region Events

		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		event EventHandler OnBookingsChanged;

		event EventHandler<GenericEventArgs<eCalendarFeatures>> OnSupportedCalendarFeaturesChagned;

		#endregion

		#region Properties

		eCalendarFeatures SupportedCalendarFeatures { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Updates the collection of bookings.
		/// </summary>
		void Refresh();

		/// <summary>
		/// Gets the collection of calendar bookings.
		/// </summary>
		IEnumerable<IBooking> GetBookings();

		/// <summary>
		/// Pushes the booking to the calendar service.
		/// </summary>
		/// <param name="booking"></param>
		void PushBooking(IBooking booking);

		/// <summary>
		/// Edits the selected booking with the calendar service.
		/// </summary>
		/// <param name="booking"></param>
		void EditBooking(IBooking booking);

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

		#endregion
	}

	[Flags]
	public enum eCalendarFeatures
	{
		None = 0,

		/// <summary>
		/// Supports listing a set of bookings.
		/// </summary>
		ListBookings = 1,

		/// <summary>
		/// Supports creating new bookings.
		/// </summary>
		CreateBookings = 2,

		/// <summary>
		/// Supports editing existing bookings.
		/// </summary>
		EditBookings = 4,

		/// <summary>
		/// Supports checking into bookings.
		/// </summary>
		CheckIn = 8,

		/// <summary>
		/// Supports checking out of bookings.
		/// </summary>
		CheckOut = 16
	}
}
