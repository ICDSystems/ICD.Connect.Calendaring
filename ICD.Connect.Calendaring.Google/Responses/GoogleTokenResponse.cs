﻿#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Connect.Calendaring.Google.Converters;

namespace ICD.Connect.Calendaring.Google.Responses
{
	[JsonConverter(typeof(GoogleTokenResponseConverter))]
	public sealed class GoogleTokenResponse : AbstractGoogleResponse
	{
		public string AccessToken { get; set; }
		public int ExpiresInSeconds { get; set; }
		public string TokenType { get; set; }
	}
}
