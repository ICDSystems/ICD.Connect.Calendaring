using ICD.Connect.Calendaring.Robin.Components.Converters;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Components.Users
{
	[JsonConverter(typeof(UserConverter))]
	public sealed class User
	{
		/// <summary>
		/// Name of the meeting
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Scheduled start time of the meeting
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Scheduled end time for the meeting
		/// </summary>
		public string TimeZone { get; set; }

		/// <summary>
		/// Name of the person who created the meeting.
		/// </summary>
		/// <remarks>
		/// From Zoom API docs: Typically empty.
		/// </remarks>
		public EmailInfo Email { get; set; }

		[JsonConverter(typeof(EmailInfoConverter))]
		public sealed class EmailInfo
		{
			public string Email { get; set; }
			public bool Verified { get; set; }
		}
	}
}
