using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.TimeZoneInfo;
using ICD.Connect.Calendaring.Asure.ResourceScheduler.Model;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Asure.Controls.Calendar
{
	public sealed class AsureBooking : AbstractBooking
	{
		private readonly ReservationData m_Reservation;
		private readonly List<IDialContext> m_DialContexts;

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
				return GetOrganizer() == null ? null : GetOrganizer().FullName;
			}
		}

		/// <summary>
		/// Returns the organizer's email.
		/// </summary>
		public override string OrganizerEmail
		{
			get
			{
				return GetOrganizer() == null ? null : GetOrganizer().EmailAddress;
			}
		}

		/// <summary>
		/// Returns the meeting start time.
		/// </summary>
		public override DateTime StartTime
		{
			get
			{
				return m_Reservation.ScheduleData.Start != null
					       ? IcdTimeZoneInfo.FindSystemTimeZoneById(m_Reservation.ScheduleData.TimeZoneId)
					                        .ConvertToUtc(m_Reservation.ScheduleData.Start.Value)
					       : DateTime.MinValue;
			}
		}

		/// <summary>
		/// Returns the meeting end time.
		/// </summary>
		public override DateTime EndTime { get
		{
			return m_Reservation.ScheduleData.End != null
				       ? IcdTimeZoneInfo.FindSystemTimeZoneById(m_Reservation.ScheduleData.TimeZoneId)
				                        .ConvertToUtc(m_Reservation.ScheduleData.End.Value)
				       : DateTime.MinValue;
		} }

		/// <summary>
		/// Returns true if meeting is private.
		/// </summary>
		public override bool IsPrivate { get { return false; } }

		/// <summary>
		/// Returns true if the booking is checked in.
		/// </summary>
		public override bool CheckedIn { get { return m_Reservation.CheckedIn; } }

		/// <summary>
		/// Returns true if the booking is checked out.
		/// </summary>
		public override bool CheckedOut { get { return m_Reservation.CheckedOut; } }

	    #endregion

	    #region Methods

	    public override IEnumerable<IDialContext> GetBookingNumbers()
	    {
	        return m_DialContexts.ToArray(m_DialContexts.Count);
	    }

	    /// <summary>
	    /// Attempts to get the organizer of the meeting
	    /// This is the CreatedForUser, or the CreatedByUser
	    /// If neither of those are set, we simply return the first attendee
	    /// </summary>
	    /// <returns></returns>
	    [CanBeNull]
	    private ReservationAttendeeData GetOrganizer()
	    {
	        return m_Reservation.ReservationBaseData.CreatedForAttendee ?? m_Reservation.ReservationBaseData.CreatedByAttendee ?? m_Reservation.ReservationBaseData.ReservationAttendees.FirstOrDefault();
	    }
		
	    #endregion

	    /// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="reservation"></param>
		/// <param name="dialContexts"></param>
		public AsureBooking(ReservationData reservation, IEnumerable<IDialContext> dialContexts)
		{
			if (reservation == null)
				throw new ArgumentNullException("reservation");

			m_Reservation = reservation;

			if (dialContexts != null)
				m_DialContexts = dialContexts.ToList();
		}
	}
}
