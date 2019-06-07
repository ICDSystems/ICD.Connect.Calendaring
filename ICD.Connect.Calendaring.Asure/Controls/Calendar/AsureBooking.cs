using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Asure.ResourceScheduler.Model;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Asure.Controls.Calendar
{
	public sealed class AsureBooking : AbstractBooking
	{
		private readonly ReservationData m_Reservation;
		private readonly List<IDialContext> m_BookingNumbers;

		#region Properties

		/// <summary>
		/// Returns the name of the meeting.
		/// </summary>
		public override string MeetingName { get { return m_Reservation.ReservationBaseData.Description; } }

		/// <summary>
		/// Returns the organizer's name.
		/// </summary>
		public override string OrganizerName
		{
			get
			{
				return m_Reservation.ReservationBaseData
				                    .ReservationAttendees
				                    .Select(a => a.FullName)
				                    .FirstOrDefault();
			}
		}

		/// <summary>
		/// Returns the organizer's email.
		/// </summary>
		public override string OrganizerEmail
		{
			get
			{
				return m_Reservation.ReservationBaseData
				                    .ReservationAttendees
				                    .Select(a => a.EmailAddress)
				                    .FirstOrDefault();
			}
		}

		/// <summary>
		/// Returns the meeting start time.
		/// </summary>
		public override DateTime StartTime { get { return m_Reservation.ScheduleData.Start ?? DateTime.MinValue; } }

		/// <summary>
		/// Returns the meeting end time.
		/// </summary>
		public override DateTime EndTime { get { return m_Reservation.ScheduleData.End ?? DateTime.MinValue; } }

		/// <summary>
		/// Returns true if meeting is private.
		/// </summary>
		public override bool IsPrivate { get { return false; } }

		public override IEnumerable<IDialContext> GetBookingNumbers()
		{
			return m_BookingNumbers.ToArray(m_BookingNumbers.Count);
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="reservation"></param>
		/// <param name="bookingProtocolInfo"></param>
		public AsureBooking(ReservationData reservation, IEnumerable<BookingProtocolInfo> bookingProtocolInfo)
		{
			if (reservation == null)
				throw new ArgumentNullException("reservation");

			m_Reservation = reservation;

			if (bookingProtocolInfo != null)
				m_BookingNumbers = ParseBookingNumbers(bookingProtocolInfo).ToList();
		}

		private static IEnumerable<IDialContext> ParseBookingNumbers(IEnumerable<BookingProtocolInfo> bookingProtocolInfo)
		{
			foreach (BookingProtocolInfo info in bookingProtocolInfo)
			{
				switch (info.DialProtocol)
				{
					case eDialProtocol.None:
						continue;

					case eDialProtocol.Sip:
						yield return new SipDialContext { DialString = info.Number };
						continue;

					case eDialProtocol.Pstn:
						yield return new PstnDialContext { DialString = info.Number };
						continue;

					case eDialProtocol.Zoom:
						yield return new ZoomDialContext { DialString = info.Number };
						continue;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}
