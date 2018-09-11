using System.Collections.Generic;
using ICD.Connect.Calendaring.Booking;

namespace ICD.Connect.Calendaring.Comparers
{
	public sealed class BookingsComparer<T> : IEqualityComparer<T>
    where T : IBooking
    {
		public bool Equals(T x, T y)
		{
			return x.MeetingName == y.MeetingName
				   && x.OrganizerName == y.OrganizerName
				   && x.OrganizerEmail == y.OrganizerEmail
				   && x.StartTime == y.StartTime
				   && x.EndTime == y.EndTime;
		}

		public int GetHashCode(T zoomBooking)
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + (zoomBooking.MeetingName == null ? 0 : zoomBooking.MeetingName.GetHashCode());
				hash = hash * 23 + (zoomBooking.OrganizerEmail == null ? 0 : zoomBooking.OrganizerEmail.GetHashCode());
				hash = hash * 23 + (zoomBooking.OrganizerName == null ? 0 : zoomBooking.OrganizerName.GetHashCode());
				hash = hash * 23 + (int)zoomBooking.StartTime.Ticks;
				hash = hash * 23 + (int)zoomBooking.EndTime.Ticks;
				return hash;
			}
		}
	}
}
