using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Comparers;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Calendaring.Booking;
using ICD.Common.Utils.Timers;
using ICD.Connect.Calendaring.CalendarControl;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Controls
{
	public sealed class Office365CalendarControl : AbstractCalendarControl<Office365CalendarDevice>
	{
		private const int TIMER_REFRESH_INTERVAL = 10 * 60 * 1000;

		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		public override event EventHandler OnBookingsChanged;
		
		private readonly IcdOrderedDictionary<CalendarEvent, Office365Booking>  m_Bookings;
		private readonly SafeCriticalSection m_BookingsSection;
		private readonly SafeTimer m_RefreshTimer;

		private static readonly PredicateComparer<CalendarEvent, DateTime> s_CalendarEventComparer;

		/// <summary>
		/// Static constructor.
		/// </summary>
		static Office365CalendarControl()
		{
			s_CalendarEventComparer = new PredicateComparer<CalendarEvent, DateTime>(e => e.Start.DateTime);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public Office365CalendarControl(Office365CalendarDevice parent, int id)
			: base(parent, id)
		{
			m_Bookings = new IcdOrderedDictionary<CalendarEvent, Office365Booking>(s_CalendarEventComparer);
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
				IEnumerable<KeyValuePair<CalendarEvent, Office365Booking>> bookings =
					Parent.GetEvents()
					      .Select(e => new KeyValuePair<CalendarEvent, Office365Booking>(e, GetBooking(e)));

				m_Bookings.Clear();
				m_Bookings.AddRange(bookings);
			}
			catch (Exception e)
			{
				Log(eSeverity.Error, "Failed to get events - {0}", e.Message);
			}
			finally
			{
				m_BookingsSection.Leave();	
			}
			
			OnBookingsChanged.Raise(this);
		}

		private Office365Booking GetBooking(CalendarEvent calendarEvent)
		{
			IEnumerable<BookingProtocolInfo> bookingNumbers =
				Parent.CalendarParserCollection
				      .ParseText(calendarEvent.Body.Content);

			return new Office365Booking(calendarEvent, bookingNumbers);
		}

		/// <summary>
		/// Gets the collection of calendar bookings.
		/// </summary>
		public override IEnumerable<IBooking> GetBookings()
		{
			return m_BookingsSection.Execute(() => m_Bookings.Cast<IBooking>().ToArray());
		}
	}
}