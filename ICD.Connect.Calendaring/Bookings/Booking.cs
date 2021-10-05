#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Bookings
{
	[JsonConverter(typeof(BookingJsonConverter))]
	public sealed class Booking : AbstractBasicBooking
	{
		/// <summary>
		/// Creates a copy of the given booking.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		[NotNull]
		public static Booking Copy([NotNull] IBooking other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			Booking output =
				new Booking
				{
					MeetingName = other.MeetingName,
					OrganizerName = other.OrganizerName,
					OrganizerEmail = other.OrganizerEmail,
					StartTime = other.StartTime,
					EndTime = other.EndTime,
					IsPrivate = other.IsPrivate,
					CheckedIn = other.CheckedIn,
					CheckedOut = other.CheckedOut
				};

			IEnumerable<IDialContext> bookingNumbers = other.GetBookingNumbers().Select(b => (IDialContext)DialContext.Copy(b));
			output.SetBookingNumbers(bookingNumbers);

			return output;
		}
	}

	public sealed class BookingJsonConverter : AbstractBasicBookingJsonConverter<Booking>
	{
	}
}
