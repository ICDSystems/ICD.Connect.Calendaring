using System.Collections.Generic;
using ICD.Connect.Calendaring.Bookings;

namespace ICD.Connect.Calendaring.Comparers
{
	/// <summary>
	/// Booking Comparer only looks at start and end times for a booking.
	/// This is strictly for de-duplication purposes.
	/// </summary>
	public sealed class BookingDeduplicationEqualityComparer : IEqualityComparer<IBooking>
	{
		private static BookingDeduplicationEqualityComparer s_Instance;

		public static BookingDeduplicationEqualityComparer Instance
		{
			get { return s_Instance ?? (s_Instance = new BookingDeduplicationEqualityComparer()); }
		}

		public bool Equals(IBooking x, IBooking y)
		{
			if (x == null && y == null)
				return true;
			if (x == null || y == null)
				return false;

			return x.StartTime == y.StartTime &&
			       x.EndTime == y.EndTime;
		}

		public int GetHashCode(IBooking obj)
		{
			int hash = 17;
			hash = hash * 23 + obj.StartTime.GetHashCode();
			hash = hash * 23 + obj.EndTime.GetHashCode();
			return hash;
		}
	}
}
