using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Components.Bookings
{
    public sealed class User
    {
		/// <summary>
		/// Name of the meeting
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; private set; }

		/// <summary>
		/// Scheduled start time of the meeting
		/// </summary>
		[JsonProperty("name")]
		public string UserName { get; private set; }

		/// <summary>
		/// Scheduled end time for the meeting
		/// </summary>
		[JsonProperty("time_zone")]
		public string TimeZone { get; private set; }

		/// <summary>
		/// Name of the person who created the meeting.
		/// </summary>
		/// <remarks>
		/// From Zoom API docs: Typically empty.
		/// </remarks>
		[JsonProperty("primary_email")]
		public EmailInfo Email { get; private set; }

	    public class EmailInfo
	    {
	        [JsonProperty("email")]
            public string Email { get; private set; }

	        [JsonProperty("is_verified")]
            public bool Verified { get; private set; }
	    }
    }
}