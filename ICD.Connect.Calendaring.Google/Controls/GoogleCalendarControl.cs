using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Comparers;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Common.Utils.Timers;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Calendaring.Google.Responses;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.Google.Controls
{
	public sealed class GoogleCalendarControl : AbstractCalendarControl<GoogleCalendarDevice>
	{
		#region Events

		/// <summary>
		/// Raised when bookings are added/removed.
		/// </summary>
		public override event EventHandler OnBookingsChanged;

		#endregion

		#region Members

		private const int TIMER_REFRESH_INTERVAL = 10 * 60 * 1000;

		private readonly IcdOrderedDictionary<GoogleCalendarEvent, GoogleBooking> m_Bookings;
		private readonly SafeCriticalSection m_BookingsSection;
		private readonly SafeTimer m_RefreshTimer;

		private static readonly PredicateComparer<GoogleCalendarEvent, DateTime> s_CalendarEventComparer;

		#endregion

		#region Constructors

		static GoogleCalendarControl()
		{
			s_CalendarEventComparer = new PredicateComparer<GoogleCalendarEvent, DateTime>(e => e.Start.DateTime);
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public GoogleCalendarControl(GoogleCalendarDevice parent, int id)
			: base(parent, id)
		{
			m_Bookings = new IcdOrderedDictionary<GoogleCalendarEvent, GoogleBooking>(s_CalendarEventComparer);
			m_BookingsSection = new SafeCriticalSection();
			m_RefreshTimer = new SafeTimer(Refresh, TIMER_REFRESH_INTERVAL, TIMER_REFRESH_INTERVAL);

			SupportedCalendarFeatures = eCalendarFeatures.ListBookings |
			                            eCalendarFeatures.CreateBookings |
			                            eCalendarFeatures.EditBookings;
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

		#endregion

		#region Methods

		/// <summary>
		/// Updates the collection of bookings.
		/// </summary>
		public override void Refresh()
		{
			m_BookingsSection.Enter();
			try
			{
				IEnumerable<KeyValuePair<GoogleCalendarEvent, GoogleBooking>> bookings =
					Parent.GetEvents().Select(e => new KeyValuePair<GoogleCalendarEvent, GoogleBooking>(e, GetBooking(e)));
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

		private GoogleBooking GetBooking(GoogleCalendarEvent calendarEvent)
		{
			IEnumerable<IDialContext> dialContexts = calendarEvent.Description == null
				                                         ? Enumerable.Empty<IDialContext>()
				                                         : Parent.CalendarParserCollection
				                                                 .ParseText(calendarEvent.Description);

			return new GoogleBooking(calendarEvent, dialContexts);
		}

		/// <summary>
		/// Gets the collection of calendar bookings.
		/// </summary>
		public override IEnumerable<IBooking> GetBookings()
		{
			return m_BookingsSection.Execute(() => m_Bookings.Values.Cast<IBooking>().ToArray());
		}

		public override void PushBooking(IBooking booking)
		{
			GoogleCalendarEvent newEvent = new GoogleCalendarEvent
			{
				Start = new GoogleCalendarEventTime
				{
					DateTime = booking.StartTime
				},
				End = new GoogleCalendarEventTime
				{
					DateTime = booking.EndTime
				},
				Summary = booking.MeetingName,
				Organizer = new GoogleCalendarEventEmail
				{
					Email = booking.OrganizerEmail
				}
			};

			Parent.CreateEvent(newEvent);
			Refresh();
		}

		public override void EditBooking(IBooking oldBooking, IBooking newBooking)
		{
			if (!(oldBooking is GoogleBooking))
				throw new InvalidOperationException(string.Format("Cannot convert booking to GoogleBooking. Booking - {0}", oldBooking));

			GoogleCalendarEvent oldEvent = m_Bookings.GetKey(oldBooking as GoogleBooking);
			string eventId = oldEvent.Id;

			int seq = int.Parse(oldEvent.Sequence);
			seq++;

			oldEvent.Sequence = seq.ToString();
			oldEvent.Start.DateTime = newBooking.StartTime;
			oldEvent.End.DateTime = newBooking.EndTime;
			oldEvent.Summary = newBooking.MeetingName;
			oldEvent.Organizer.Email = newBooking.OrganizerEmail;

			Parent.EditEvent(oldEvent, eventId);
			Refresh();
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

		#endregion
	}
}
