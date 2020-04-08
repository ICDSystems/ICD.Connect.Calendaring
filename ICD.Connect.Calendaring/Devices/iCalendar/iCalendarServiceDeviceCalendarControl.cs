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
using ICD.Connect.Calendaring.Devices.iCalendar.Parser;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Devices.iCalendar
{
// ReSharper disable InconsistentNaming
	public sealed class iCalendarServiceDeviceCalendarControl : AbstractCalendarControl<iCalendarServiceDevice>
// ReSharper restore InconsistentNaming
	{
		private const int TIMER_REFRESH_INTERVAL = 10 * 60 * 1000;

		public override event EventHandler OnBookingsChanged;
		private readonly IcdOrderedDictionary<iCalendarEvent, iCalendarBooking> m_Bookings;
		private readonly SafeTimer m_RefreshTimer;
		private readonly SafeCriticalSection m_BookingSection;

		private static readonly PredicateComparer<iCalendarEvent, DateTime> s_CalendarEventComparer;

		/// <summary>
		/// Static constructor.
		/// </summary>
		static iCalendarServiceDeviceCalendarControl()
		{
			s_CalendarEventComparer = new PredicateComparer<iCalendarEvent, DateTime>(e => e.DtStart);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public iCalendarServiceDeviceCalendarControl(iCalendarServiceDevice parent, int id)
			: base(parent, id)
		{
			m_Bookings = new IcdOrderedDictionary<iCalendarEvent, iCalendarBooking>(s_CalendarEventComparer);
			m_BookingSection = new SafeCriticalSection();
			m_RefreshTimer = new SafeTimer(Refresh, TIMER_REFRESH_INTERVAL, TIMER_REFRESH_INTERVAL);
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public override void Refresh()
		{
			m_BookingSection.Enter();

			try
			{
				iCalendarCalendar calendar = Parent.GetCalendar();

				IEnumerable<KeyValuePair<iCalendarEvent, iCalendarBooking>> bookings =
					calendar == null
						? Enumerable.Empty<KeyValuePair<iCalendarEvent, iCalendarBooking>>()
						: calendar.GetEvents().Select(e => new KeyValuePair<iCalendarEvent, iCalendarBooking>(e, GetBookings(e)));

				m_Bookings.Clear();
				m_Bookings.AddRange(bookings);
			}
			catch (Exception e)
			{
				Logger.Log(eSeverity.Error, "Failed to get events - {0}", e.Message);
			}
			finally
			{
				m_BookingSection.Leave();
			}

			OnBookingsChanged.Raise(this);
		}

		private iCalendarBooking GetBookings(iCalendarEvent iCalendarEvent)
		{
			IEnumerable<IDialContext> bookingNumbers =
				Parent.CalendarParserCollection.ParseText(iCalendarEvent.Description);

			return new iCalendarBooking(iCalendarEvent, bookingNumbers);
		}

		/// <summary>
		/// Gets the collection of calendar bookings.
		/// </summary>
		public override IEnumerable<IBooking> GetBookings()
		{
			return m_BookingSection.Execute(() => m_Bookings.Values.ToArray(m_Bookings.Count));
		}

		public override bool CanCheckIn(IBooking booking)
		{
			return false;
		}

		public override bool CanCheckOut(IBooking booking)
		{
			return false;
		}

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