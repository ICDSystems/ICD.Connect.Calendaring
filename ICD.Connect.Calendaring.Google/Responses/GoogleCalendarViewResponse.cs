using System;
using System.Collections.Generic;
using System.Text;
using ICD.Connect.Calendaring.Google.Converters;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Responses
{
	[JsonConverter(typeof(GoogleCalendarViewResponseConverter))]
	public sealed class GoogleCalendarViewResponse : AbstractGoogleResponse
	{
		public string Kind { get; set; }
		public string ETag { get; set; }
		public string Summary { get; set; }
		public DateTime Updated { get; set; }
		public string TimeZone { get; set; }
		public string AccessRole { get; set; }
		public GoogleDefaultReminder[] DefaultReminders { get; set; }
		public string NextSyncToken { get; set; }
		public GoogleCalendarEvent[] Items { get; set; } 
	}

	[JsonConverter(typeof(GoogleDefaultReminderConverter))]
	public sealed class GoogleDefaultReminder
	{

	}

	[JsonConverter(typeof(GoogleCalendarEventConverter))]
	public sealed class GoogleCalendarEvent
	{
		public string Kind { get; set; }
		public string ETag { get; set; }
		public string Id { get; set; }
		public string Status { get; set; }
		public string HtmlLink { get; set; }
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
		public string Summary { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public GoogleCalendarEventEmail Creator { get; set; }
		public GoogleCalendarEventEmail Organizer { get; set; }
		public GoogleCalendarEventTime Start { get; set; }
		public GoogleCalendarEventTime End { get; set; }
		public string ICalUid { get; set; }
		public string Sequence { get; set; }
		public GoogleCalendarEventAttendee[] Attendees { get; set; }
	
		public bool GuestCanModify { get; set; }
		public GoogleCalendarEventReminder Reminders { get; set; }


	}

	[JsonConverter(typeof(GoogleCalendarEventEmailConverter))]
	public sealed class GoogleCalendarEventEmail
	{
		public string Email { get; set; }
	}


	[JsonConverter(typeof(GoogleCalendarEventTimeConverter))]
	public sealed class GoogleCalendarEventTime
	{
		public DateTime DateTime { get; set; }
	}

	[JsonConverter(typeof(GoogleCalendarEventAttendeeConverter))]
	public sealed class GoogleCalendarEventAttendee
	{
		public string Email { get; set; }
		public bool Self { get; set; }
		public bool Optional { get; set; }
		public string DisplayName { get; set; }
		public string Resource { get; set; }
		public bool Organizer { get; set; }
		public string ResponseStatus { get; set; }
	}

	[JsonConverter(typeof(GoogleCalendarEventReminderConverter))]
	public sealed class GoogleCalendarEventReminder
	{
		public bool UseDefault { get; set; }
	}
}

