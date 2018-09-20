using System;
using ICD.Common.Properties;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Components.Bookings
{
	[UsedImplicitly]
	public sealed class Event
	{
		/// <summary>
		/// Name of the meeting
		/// </summary>
		[JsonProperty("title")]
		public string MeetingName { get; private set; }

		/// <summary>
		/// Scheduled start time of the meeting
		/// </summary>
		[JsonProperty("start")]
		public DateInfo MeetingStart { get; private set; }

		/// <summary>
		/// Scheduled end time for the meeting
		/// </summary>
		[JsonProperty("end")]
		public DateInfo MeetingEnd { get; private set; }

		/// <summary>
		/// ID of the person who created the meeting.
		/// </summary>
		[JsonProperty("creator_id")]
		public string OrganizerId{ get; private set; }

	    /// <summary>
	    /// Name of the person who created the meeting.
	    /// </summary>
	    /// <remarks>
	    /// Not returned in Robin events response.
	    /// </remarks>
	    public string OrganizerName { get; internal set; }

        /// <summary>
        /// Email of the person who created the meeting
        /// </summary>
        [JsonProperty("creator_email")]
		public string OrganizerEmail { get; private set; }

		/// <summary>
		/// Zoom meeting id to join for the meeting
		/// </summary>
		[JsonProperty("id")]
		public string MeetingNumber { get; private set; }

	    /// <summary>
	    /// Zoom meeting id to join for the meeting
	    /// </summary>
	    [JsonProperty("description")]
	    public string Description { get; private set; }

        /// <summary>
        /// Zoom meeting id to join for the meeting
        /// </summary>
        [JsonProperty("visibility")]
	    public string IsPrivate { get; private set; }

		[UsedImplicitly]
		public sealed class DateInfo
	    {
	        [JsonProperty("date_time")]
            public DateTime DateTimeInfo { get; private set; }

	        [JsonProperty("time_zone")]
            public string TimeZoneInfo { get; private set; }
	    }
    }
}