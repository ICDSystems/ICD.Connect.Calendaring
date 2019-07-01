using System;
using ICD.Connect.Calendaring.Microsoft.Office365.Converters;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Responses
{
	[JsonConverter(typeof(CalendarViewResponseConverter))]
	public sealed class CalendarViewResponse : AbstractResponse
	{
		public string DataContext { get; set; }
		public CalendarEvent[] Value { get; set; }
		
	}

	[JsonConverter(typeof(CalendarEventConverter))]
	public sealed class CalendarEvent
	{
		public string ODataEtag { get; set; }
		public string Id { get; set; }
		public string CreatedDateTime { get; set; }
		public string LastModifiedDateTime { get; set; }
		public string ChangeKey { get; set; }
		public string[] Categories { get; set; }
		public string OriginalStartTimeZone { get; set; }
		public string OriginalEndTimeZone { get; set; }
// ReSharper disable InconsistentNaming
		public string ICalUId { get; set; }
// ReSharper restore InconsistentNaming
		public int ReminderMinutesBeforeStart { get; set; }
		public bool IsReminderOn { get; set; }
		public bool HasAttachments { get; set; }
		public string Subject { get; set; }
		public string BodyPreview { get; set; }
		public string Importance { get; set; }
		public string Sensitivity { get; set; }
		public bool IsAllDay { get; set; }
		public bool IsCancelled { get; set; }
		public bool IsOrganizer { get; set; }
		public bool ResponseRequested { get; set; }
		public string SeriesMasterId { get; set; }
		public string ShowAs { get; set; }
		public string Type { get; set; }
		public string WebLink { get; set; }
		public string OnlineMeetingUrl { get; set; }
		public string Recurence { get; set; } // TODO - Make a recurring meeting
		public CalendarEventResponseStatus ResponseStatus { get; set; }
		public CalendarEventBody Body { get; set; }
		public CalendarEventTime Start { get; set; }
		public CalendarEventTime End { get; set; }
		public CalendarEventLocation Location { get; set; }
		public CalendarEventLocation[] Locations { get; set; }
		public CalendarEventAttendees[] Attendees { get; set; } // TODO - Add some attendees
		public CalendarEventOrganizer Organizer { get; set; }
	}

	[JsonConverter(typeof(CalendarEventResponseStatusConverter))]
	public sealed class CalendarEventResponseStatus
	{
		public string Response { get; set; }
		public string Time { get; set; }

	}

	[JsonConverter(typeof(CalendarEventBodyConverter))]
	public sealed class CalendarEventBody
	{
		public string ContentType { get; set; }
		public string Content { get; set; }
	}


	[JsonConverter(typeof(CalendarEventTimeConverter))]
	public sealed class CalendarEventTime
	{
		public DateTime DateTime { get; set; }
		public string TimeZone { get; set; }
	}

	[JsonConverter(typeof(CalendarEventLocationConverter))]
	public sealed class CalendarEventLocation
	{
		public string DisplayName { get; set; }
		public string LocationType { get; set; }
		public string UniqueId { get; set; }
		public string UniqueIdType { get; set; }
		public CalendarEventLocationAddress Address { get; set; }
		public string Coordinates { get; set; }
	}
	[JsonConverter(typeof(CalendarEventLocationAddressConverter))]
	public sealed class CalendarEventLocationAddress
	{
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string CountryOrRegion { get; set; }
		public string PostalCode { get; set; }
	}

	[JsonConverter(typeof(CalendarEventAttendeesConverter))]
	public sealed class CalendarEventAttendees
	{
		public string Type { get; set; }
		public AttendeesStatus Status { get; set; }
		public AttendeesEmail EmailAdress { get; set; }
		
	}
	[JsonConverter(typeof(CalendarEventAttendeesEmailConverter))]
	public class AttendeesEmail
	{
		public string Name { get; set; }
		public string Address { get; set; }
	}

	[JsonConverter(typeof(CalendarEventAttendeesStatusConverter))]
	public class AttendeesStatus
	{
		public string Response { get; set; }
		public string Time { get; set; }
	}

	[JsonConverter(typeof(CalendarEventOrganizerConverter))]
	public sealed class CalendarEventOrganizer
	{
		public CalendarEventEmailAddress EmailAddress { get; set; }
	}

	[JsonConverter(typeof(CalendarEventEmailAddressConverter))]
	public sealed class CalendarEventEmailAddress
	{
		public string Name { get; set; }
		public string Address { get; set; }
	}

}