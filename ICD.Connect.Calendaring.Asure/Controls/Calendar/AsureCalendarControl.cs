using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Comparers;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Asure.ResourceScheduler.Model;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Asure.Controls.Calendar
{
	public sealed class AsureCalendarControl : AbstractCalendarControl<AsureDevice>
	{
		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		public override event EventHandler OnBookingsChanged;

		private readonly IcdSortedDictionary<ReservationData, AsureBooking> m_ReservationToBooking;
		private readonly SafeCriticalSection m_BookingSection;

		/// <summary>
		/// Sort reservations by start time.
		/// </summary>
		private static readonly PredicateComparer<ReservationData, DateTime> s_ReservationComparer;

		/// <summary>
		/// Static constructor.
		/// </summary>
		static AsureCalendarControl()
		{
			s_ReservationComparer = new PredicateComparer<ReservationData, DateTime>(r => r.ScheduleData.Start ?? DateTime.MinValue);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public AsureCalendarControl(AsureDevice parent, int id)
			: base(parent, id)
		{
			m_ReservationToBooking = new IcdSortedDictionary<ReservationData, AsureBooking>(s_ReservationComparer);
			m_BookingSection = new SafeCriticalSection();

			SupportedCalendarFeatures = eCalendarFeatures.ListBookings |
			                            eCalendarFeatures.CreateBookings |
			                            eCalendarFeatures.EditBookings |
			                            eCalendarFeatures.CheckIn |
			                            eCalendarFeatures.CheckOut;

			parent.OnCacheUpdated += ParentOnCacheUpdated;
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnBookingsChanged = null;

			Parent.OnCacheUpdated -= ParentOnCacheUpdated;

			base.DisposeFinal(disposing);
		}

		#region Methods

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override void Refresh()
		{
			Parent.RefreshCache();
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override IEnumerable<IBooking> GetBookings()
		{
			return m_BookingSection.Execute(() => m_ReservationToBooking.Values.ToArray(m_ReservationToBooking.Count));
		}

		/// <summary>
		/// Pushes the booking to the calendar service.
		/// </summary>
		/// <param name="booking"></param>
		public override void PushBooking(IBooking booking)
		{
			Parent.SubmitReservation(booking.MeetingName, "", booking.StartTime, booking.EndTime);
		}

		/// <summary>
		/// Edits the selected booking with the calendar service.
		/// </summary>
		/// <param name="oldBooking"></param>
		/// <param name="newBooking"></param>
		public override void EditBooking(IBooking oldBooking, IBooking newBooking)
		{
			ReservationData res = m_ReservationToBooking.FirstOrDefault(kvp => kvp.Value == oldBooking).Key;
			if (res == null)
				throw new InvalidOperationException("No reservation data associated with the specified booking");

			Parent.SubmitReservation(newBooking.MeetingName, "", newBooking.StartTime, newBooking.EndTime);
		}

		/// <summary>
		/// Returns true if the booking argument can be checked in.
		/// </summary>
		/// <returns></returns>
		public override bool CanCheckIn(IBooking booking)
		{
			return Parent.CanCheckIn();
		}

		/// <summary>
		/// Returns true if the booking argument can be checked out of.
		/// </summary>
		/// <param name="booking"></param>
		/// <returns></returns>
		public override bool CanCheckOut(IBooking booking)
		{
			return Parent.CanCheckOut();
		}

		/// <summary>
		/// Checks in to the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		public override void CheckIn(IBooking booking)
		{
			if (!CanCheckIn(booking))
				throw new ArgumentException("The specified booking does not support check ins.", "booking");

			var reservationData = m_ReservationToBooking.FirstOrDefault(b => b.Value == booking).Key;
			Parent.CheckIn(reservationData.ReservationBaseData.Id);
		}

		/// <summary>
		/// Checks out of the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		public override void CheckOut(IBooking booking)
		{
			if (!CanCheckOut(booking))
				throw new ArgumentException("The specified booking does not support check outs.", "booking");

			var reservationData = m_ReservationToBooking.FirstOrDefault(b => b.Value == booking).Key;
			Parent.CheckOut(reservationData.ReservationBaseData.Id);
		}

		#endregion

		#region Parent Callbacks

		private void ParentOnCacheUpdated(object sender, EventArgs eventArgs)
		{
			bool change = false;

			ReservationData[] reservations =
				Parent.GetReservations()
				      .Where(r => r.ScheduleData.End != null &&
				                  AsureDevice.GetScheduleDataDateTimeUtc(r.ScheduleData.End.Value,
				                                                    r.ScheduleData.TimeZoneId) >
				                  IcdEnvironment.GetUtcTime())
				      .Distinct()
				      .ToArray();

			m_BookingSection.Enter();

			try
			{
				IcdHashSet<ReservationData> existing = m_ReservationToBooking.Keys.ToIcdHashSet();
				IcdHashSet<ReservationData> removeBookingList = existing.Subtract(reservations);

				foreach (ReservationData reservation in removeBookingList)
					change |= RemoveBooking(reservation);

				foreach (ReservationData reservation in reservations)
					change |= AddBooking(reservation);
			}
			finally
			{
				m_BookingSection.Leave();
			}

			if (change)
				OnBookingsChanged.Raise(this);
		}

		#endregion

		#region Private Methods

		private bool AddBooking(ReservationData reservation)
		{
			m_BookingSection.Enter();

			try
			{
				if (m_ReservationToBooking.ContainsKey(reservation))
					return false;

				IEnumerable<IDialContext> bookingProtocolInfos =
					Parent.CalendarParserCollection.ParseText(reservation.ReservationBaseData.Notes);
				AsureBooking booking = new AsureBooking(reservation, bookingProtocolInfos);

				m_ReservationToBooking.Add(reservation, booking);
			}
			finally
			{
				m_BookingSection.Leave();
			}

			return true;
		}

		private bool RemoveBooking(ReservationData booking)
		{
			return m_BookingSection.Execute(() => m_ReservationToBooking.Remove(booking));
		}

		#endregion
	}
}
