using System.Collections.Generic;

namespace ICD.Connect.Calendaring.Bookings
{
	/// <summary>
	/// Booking Comparer only looks at start and end times for a booking.
	/// This is strictly for de-duplication purposes.
	/// </summary>
	public sealed class BookingEqualityComparer : IEqualityComparer<IBooking>
	{
		private static BookingEqualityComparer s_Instance;

		public static BookingEqualityComparer Instance
		{
			get { return s_Instance ?? (s_Instance = new BookingEqualityComparer()); }
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
