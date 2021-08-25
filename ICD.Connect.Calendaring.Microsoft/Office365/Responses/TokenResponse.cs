#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Connect.Calendaring.Microsoft.Office365.Converters;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Responses
{
	[JsonConverter(typeof(TokenResponseConverter))]
	public sealed class TokenResponse : AbstractResponse
	{
		public string TokenType { get; set; }
		public int ExpiresInSeconds { get; set; }
		public int ExtensionExpiresInSeconds { get; set; }
		public string AccessToken { get; set; }
	}
}