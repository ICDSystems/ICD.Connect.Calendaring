﻿using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Calendaring.Comparers;
using ICD.Connect.Conferencing.DialContexts;
using System;
using System.Collections.Generic;

namespace ICD.Connect.Calendaring.Bookings
{
	public interface IBooking
	{
		/// <summary>
		/// Returns the name of the meeting.
		/// </summary>
		string MeetingName { get; }

        /// <summary>
        /// Returns the organizer's name.
        /// </summary>
        string OrganizerName { get; }

		/// <summary>
		/// Returns the organizer's email.
		/// </summary>
		string OrganizerEmail { get; }

		/// <summary>
		/// Returns the meeting start time.
		/// </summary>
		DateTime StartTime { get; }

		/// <summary>
		/// Returns the meeting end time.
		/// </summary>
		DateTime EndTime { get; }

		/// <summary>
		/// Returns true if meeting is private.
		/// </summary>
		bool IsPrivate { get; }

		/// <summary>
		/// Returns true if the booking is checked in.
		/// </summary>
		bool CheckedIn { get; }

		/// <summary>
		/// Returns true if the booking is checked out.
		/// </summary>
		bool CheckedOut { get; }

		/// <summary>
		/// Returns Booking Numbers.
		/// </summary>
		IEnumerable<IDialContext> GetBookingNumbers();
	}

	public static class BookingExtensions
	{
		public static bool IsBookingCurrent([NotNull] this IBooking extends)
		{
			var now = IcdEnvironment.GetUtcTime();

			return extends.StartTime <= now && extends.EndTime > now;
		}

		public static bool Duplicates([NotNull] this IBooking extends, [CanBeNull] IBooking other)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return BookingDeduplicationEqualityComparer.Instance.Equals(extends, other);
		}
		
		public static bool IsBookingStarted([NotNull] this IBooking extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.StartTime <= DateTime.UtcNow;
		}

		public static bool IsBookingEnded([NotNull] this IBooking extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.EndTime <= DateTime.UtcNow;
		}
	}
}
