using System.Collections.Generic;
using System.Linq;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Comparers
{
	public sealed class BookingEqualityComparer : EqualityComparer<IBooking>
	{
		private static BookingEqualityComparer s_Instance;

		public static BookingEqualityComparer Instance
		{
			get { return s_Instance ?? (s_Instance = new BookingEqualityComparer()); }
		}

		public override bool Equals(IBooking x, IBooking y)
		{
			if (x == null && y == null)
				return true;

			if (x == null || y == null)
				return false;

			return x.MeetingName == y.MeetingName &&
			       x.OrganizerName == y.OrganizerName &&
			       x.OrganizerEmail == y.OrganizerEmail &&
			       x.StartTime == y.StartTime &&
			       x.EndTime == y.EndTime &&
			       x.IsPrivate == y.IsPrivate &&
			       x.CheckedIn == y.CheckedIn &&
			       x.CheckedOut == y.CheckedOut &&
			       x.GetBookingNumbers().SequenceEqual(y.GetBookingNumbers(), DialContextEqualityComparer.Instance);
		}

		public override int GetHashCode(IBooking obj)
		{
			unchecked
			{
				int hash = 17;

				hash = hash * 23 + (obj.MeetingName == null ? 0 : obj.MeetingName.GetHashCode());
				hash = hash * 23 + (obj.OrganizerName == null ? 0 : obj.OrganizerName.GetHashCode());
				hash = hash * 23 + (obj.OrganizerEmail == null ? 0 : obj.OrganizerEmail.GetHashCode());
				hash = hash * 23 + obj.StartTime.GetHashCode();
				hash = hash * 23 + obj.EndTime.GetHashCode();
				hash = hash * 23 + obj.IsPrivate.GetHashCode();
				hash = hash * 23 + obj.CheckedIn.GetHashCode();
				hash = hash * 23 + obj.CheckedOut.GetHashCode();

				foreach (IDialContext context in obj.GetBookingNumbers())
					hash = hash * 23 + DialContextEqualityComparer.Instance.GetHashCode(context);

				return hash;
			}
		}
	}
}
