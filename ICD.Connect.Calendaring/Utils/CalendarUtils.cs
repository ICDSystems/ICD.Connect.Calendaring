using System;
using ICD.Common.Properties;
using ICD.Connect.Calendaring.Bookings;

namespace ICD.Connect.Calendaring.Utils
{
	public static class CalendarUtils
	{
		/// <summary>
		/// Returns true if the given booking takes place, at least partially, between the given dates.
		/// </summary>
		/// <param name="booking"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static bool IsInRange([NotNull] IBooking booking, DateTime from, DateTime to)
		{
			if (booking == null)
				throw new ArgumentNullException("booking");

			return booking.StartTime <= to && from < booking.EndTime;
		}
	}
}
