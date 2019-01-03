using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Comparers;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Asure.ResourceScheduler.Model;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.CalendarControl;

namespace ICD.Connect.Calendaring.Asure.Controls.Calendar
{
	public sealed class AsureCalendarControl : AbstractCalendarControl<AsureDevice>
	{
		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		public override event EventHandler OnBookingsChanged;

		private readonly IcdOrderedDictionary<ReservationData, AsureBooking> m_ReservationToBooking;
		private readonly SafeCriticalSection m_BookingSection;

		/// <summary>
		/// Sort reservations by start time.
		/// </summary>
		private static readonly PredicateComparer<ReservationData, DateTime> s_ReservationComparer;

		/// <summary>
		/// Compare reservations by id.
		/// </summary>
		private static readonly PredicateEqualityComparer<ReservationData, int> s_ReservationEqualityComparer; 

		/// <summary>
		/// Static constructor.
		/// </summary>
		static AsureCalendarControl()
		{
			s_ReservationComparer = new PredicateComparer<ReservationData, DateTime>(r => r.ScheduleData.Start ?? DateTime.MinValue);
			s_ReservationEqualityComparer = new PredicateEqualityComparer<ReservationData, int>(r => r.ReservationBaseData.Id);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public AsureCalendarControl(AsureDevice parent, int id)
			: base(parent, id)
		{
			m_ReservationToBooking = new IcdOrderedDictionary<ReservationData, AsureBooking>(s_ReservationComparer, s_ReservationEqualityComparer);
			m_BookingSection = new SafeCriticalSection();

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

		#endregion

		private void ParentOnCacheUpdated(object sender, EventArgs eventArgs)
		{
			bool change = false;

			ReservationData[] reservations =
				Parent.GetReservations()
				      .Where(r => r.ScheduleData.End > IcdEnvironment.GetLocalTime())
				      .Distinct()
				      .ToArray();

			m_BookingSection.Enter();

			try
			{
				IcdHashSet<ReservationData> existing = m_ReservationToBooking.Keys.ToIcdHashSet(s_ReservationEqualityComparer);
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

		#region Private Methods

		private bool AddBooking(ReservationData reservation)
		{
			m_BookingSection.Enter();

			try
			{
				if (m_ReservationToBooking.ContainsKey(reservation))
					return false;

				AsureBooking booking = new AsureBooking(reservation);

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
