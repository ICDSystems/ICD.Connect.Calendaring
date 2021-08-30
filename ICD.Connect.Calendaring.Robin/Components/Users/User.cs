#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Connect.Calendaring.Robin.Components.Converters;

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
		/// The name of the user
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// The timezone of the user
		/// </summary>
		public string TimeZone { get; set; }

		/// <summary>
		/// Email info of the user
		/// </summary>
		public EmailInfo Email { get; set; }

		[JsonConverter(typeof(EmailInfoConverter))]
		public sealed class EmailInfo
		{
			public string Email { get; set; }
			public bool Verified { get; set; }
		}
	}
}
