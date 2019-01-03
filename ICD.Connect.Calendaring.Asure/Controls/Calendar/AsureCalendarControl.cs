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
using ICD.Connect.Calendaring.Comparers;

namespace ICD.Connect.Calendaring.Asure.Controls.Calendar
{
	public sealed class AsureCalendarControl : AbstractCalendarControl<AsureDevice>
	{
		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		public override event EventHandler OnBookingsChanged;

		private readonly List<AsureBooking> m_SortedBookings;
		private readonly IcdHashSet<AsureBooking> m_HashBooking;
		private readonly SafeCriticalSection m_BookingSection;

		/// <summary>
		/// Sort events by start time.
		/// </summary>
		private static readonly PredicateComparer<AsureBooking, DateTime> s_BookingComparer;

		/// <summary>
		/// Static constructor.
		/// </summary>
		static AsureCalendarControl()
		{
			s_BookingComparer = new PredicateComparer<AsureBooking, DateTime>(b => b.StartTime);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public AsureCalendarControl(AsureDevice parent, int id)
			: base(parent, id)
		{
			m_SortedBookings = new List<AsureBooking>();
			m_HashBooking = new IcdHashSet<AsureBooking>(new BookingsComparer<AsureBooking>());
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
			return m_BookingSection.Execute(() => m_SortedBookings.ToArray(m_SortedBookings.Count));
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
				IcdHashSet<AsureBooking> existing = m_SortedBookings.ToIcdHashSet(new BookingsComparer<AsureBooking>());
				IcdHashSet<AsureBooking> current =
					reservations.Select(r => new AsureBooking(r))
					            .ToIcdHashSet(new BookingsComparer<AsureBooking>());

				IcdHashSet<AsureBooking> removeBookingList = existing.Subtract(current);
				foreach (AsureBooking booking in removeBookingList)
					change |= RemoveBooking(booking);

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
				AsureBooking robinBooking = new AsureBooking(reservation);

				if (m_HashBooking.Contains(robinBooking))
					return false;

				m_HashBooking.Add(robinBooking);

				m_SortedBookings.AddSorted(robinBooking, s_BookingComparer);
			}
			finally
			{
				m_BookingSection.Leave();
			}

			return true;
		}

		private bool RemoveBooking(AsureBooking booking)
		{
			m_BookingSection.Enter();

			try
			{
				if (!m_HashBooking.Contains(booking))
					return false;

				m_HashBooking.Remove(booking);
				m_SortedBookings.Remove(booking);
			}
			finally
			{
				m_BookingSection.Leave();
			}

			return true;
		}

		#endregion
	}
}
