using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Comparers;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Common.Utils.Timers;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Conferencing.DialContexts;
using Independentsoft.Exchange;

namespace ICD.Connect.Calendaring.Microsoft.Exchange.Controls
{
	public sealed class ExchangeCalendarControl : AbstractCalendarControl<ExchangeCalendarDevice>
	{
		private const int TIMER_REFRESH_INTERVAL = 10 * 60 * 1000;
		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		public override event EventHandler OnBookingsChanged;

		private readonly IcdOrderedDictionary<Appointment, ExchangeBooking> m_Bookings;
		private readonly SafeCriticalSection m_BookingsSection;
		private readonly SafeTimer m_RefreshTimer;

		private static readonly PredicateComparer<Appointment, DateTime> s_CalendarEventComparer;

		static ExchangeCalendarControl()
		{
			s_CalendarEventComparer = new PredicateComparer<Appointment, DateTime>(e => e.StartTime);
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public ExchangeCalendarControl(ExchangeCalendarDevice parent, int id)
			: base(parent, id)
		{
			m_Bookings = new IcdOrderedDictionary<Appointment, ExchangeBooking>(s_CalendarEventComparer);
			m_BookingsSection = new SafeCriticalSection();
			m_RefreshTimer = new SafeTimer(Refresh, TIMER_REFRESH_INTERVAL, TIMER_REFRESH_INTERVAL);
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnBookingsChanged = null;
			m_RefreshTimer.Dispose();

			base.DisposeFinal(disposing);
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override void Refresh()
		{
			m_BookingsSection.Enter();
			try
			{
				IEnumerable<KeyValuePair<Appointment, ExchangeBooking>> bookings =
					Parent.GetAppointments().Select(e => new KeyValuePair<Appointment, ExchangeBooking>(e, GetBooking(e)));
				m_Bookings.Clear();
				m_Bookings.AddRange(bookings);

			}
			catch (Exception e)
			{
				Logger.Log(eSeverity.Error, "Failed to get events - {0}", e.Message);
			}
			finally
			{
				m_BookingsSection.Leave();
			}
			OnBookingsChanged.Raise(this);
		}

		private ExchangeBooking GetBooking(Appointment calendarEvent)
		{
			IEnumerable<IDialContext> dialContexts =
				Parent.CalendarParserCollection
						.ParseText(calendarEvent.Subject);

			return new ExchangeBooking(calendarEvent, dialContexts);
		}

		/// <summary>
		/// Gets the collection of calendar bookings.
		/// </summary>
		public override IEnumerable<IBooking> GetBookings()
		{
			return m_BookingsSection.Execute(() => m_Bookings.Values.Cast<IBooking>().ToArray());
		}

		/// <summary>
		/// Returns true if the booking argument can be checked in.
		/// </summary>
		/// <returns></returns>
		public override bool CanCheckIn(IBooking booking)
		{
			return false;
		}

		public override bool CanCheckOut(IBooking booking)
		{
			return false;
		}

		/// <summary>
		/// Checks in to the specified booking.
		/// </summary>
		/// <param name="booking"></param>
		public override void CheckIn(IBooking booking)
		{
			throw new NotSupportedException();
		}

		public override void CheckOut(IBooking booking)
		{
			throw new NotSupportedException();
		}
	}
}
