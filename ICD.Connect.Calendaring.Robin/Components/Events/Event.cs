using System;
using ICD.Connect.Calendaring.Robin.Components.Converters;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Components.Events
{
	[JsonConverter(typeof(EventConverter))]
	public sealed class Event
	{
		/// <summary>
		/// Name of the meeting
		/// </summary>
		public string MeetingName { get; set; }

		/// <summary>
		/// Scheduled start time of the meeting
		/// </summary>
		public DateInfo MeetingStart { get; set; }

		/// <summary>
		/// Scheduled end time for the meeting
		/// </summary>
		public DateInfo MeetingEnd { get; set; }

		/// <summary>
		/// ID of the person who created the meeting.
		/// </summary>
		public string OrganizerId { get; set; }

		/// <summary>
		/// Name of the person who created the meeting.
		/// </summary>
		/// <remarks>
		/// Not returned in Robin events response.
		/// </remarks>
		public string OrganizerName { get; set; }

		/// <summary>
		/// Email of the person who created the meeting
		/// </summary>
		public string OrganizerEmail { get; set; }

		/// <summary>
		/// Event id.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Zoom meeting id to join for the meeting
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Zoom meeting id to join for the meeting
		/// </summary>
		public string IsPrivate { get; set; }

		[JsonConverter(typeof(DateInfoConverter))]
		public sealed class DateInfo
		{
			public DateTime DateTimeInfo { get; set; }
			public string TimeZoneInfo { get; set; }
		}
	}
}
