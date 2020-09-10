using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Calendaring.CalendarPoints;
using ICD.Connect.Calendaring.Controls;

namespace ICD.Connect.Calendaring.CalendarManagers
{
	public interface ICalendarManager
	{
		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		event EventHandler OnBookingsChanged;

		/// <summary>
		/// Gets the registered calendar providers.
		/// </summary>
		/// <returns></returns>
		[NotNull]
		IEnumerable<ICalendarControl> GetProviders();

		/// <summary>
		/// Gets the registered calendar providers where both the calendar point and calendar control
		/// instersect with the given features mask.
		/// </summary>
		/// <returns></returns>
		[NotNull]
		IEnumerable<ICalendarControl> GetProviders(eCalendarFeatures features);

		/// <summary>
		/// Refreshes the collection of bookings on the manager.
		/// </summary>
		void RefreshBookings();

		/// <summary>
		/// Pushes the specified booking to the most appropriate provider.
		/// </summary>
		/// <param name="booking"></param>
		void PushBooking([NotNull] IBooking booking);

		/// <summary>
		/// Checks into the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		void CheckIn([NotNull] IBooking booking);

		/// <summary>
		/// Checks out of the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		void CheckOut([NotNull] IBooking booking);

		/// <summary>
		/// Determines if the specified booking can be checked into.
		/// </summary>
		/// <param name="booking"></param>
		bool CanCheckIn([NotNull] IBooking booking);

		/// <summary>
		/// Determines if the specified booking can be checked out of.
		/// </summary>
		/// <param name="booking"></param>
		/// <returns></returns>
		bool CanCheckOut([NotNull] IBooking booking);

		/// <summary>
		/// Registers the calendar provider at the given calendar point.
		/// </summary>
		/// <param name="calendarPoint"></param>
		void RegisterCalendarProvider([NotNull] ICalendarPoint calendarPoint);

		/// <summary>
		/// Deregisters the calendar provider at the given calendar point.
		/// </summary>
		/// <param name="calendarPoint"></param>
		void DeregisterCalendarProvider([NotNull] ICalendarPoint calendarPoint);

		/// <summary>
		/// Gets the available bookings.
		/// </summary>
		/// <returns></returns>
		[NotNull]
		IEnumerable<IBooking> GetBookings();

		/// <summary>
		/// Sets the calendar manager back to it's initial state.
		/// </summary>
		void Clear();
	}

	public static class CalendarManagerExtensions
	{
		/// <summary>
		/// Gets the currently active booking if there is one.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		[CanBeNull]
		public static IBooking GetCurrentBooking([NotNull] this ICalendarManager extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.GetBookings().FirstOrDefault(BookingExtensions.IsBookingCurrent);
		}

		/// <summary>
		/// Gets the next scheduled booking (excluding the current booking) if there is one.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		[CanBeNull]
		public static IBooking GetNextBooking([NotNull] this ICalendarManager extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.GetBookings()
			              .Where(b => !b.IsBookingCurrent() && b.StartTime > IcdEnvironment.GetUtcTime())
			              .OrderBy(b => b.StartTime)
			              .FirstOrDefault();
		}

		/// <summary>
		/// Gets the amount of time until the next booking starts.
		/// </summary>
		/// <returns></returns>
		public static TimeSpan GetTimeToNextBooking([NotNull] this ICalendarManager extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			IBooking next = extends.GetNextBooking();
			return next == null ? TimeSpan.MaxValue : next.StartTime - IcdEnvironment.GetUtcTime();
		}
	}
}