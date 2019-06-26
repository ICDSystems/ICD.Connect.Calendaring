using ICD.Connect.Calendaring.Microsoft.Office365.Converters;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Responses
{
	[JsonConverter(typeof(TokenResponseConverter))]
	public sealed class TokenResponse
	{
		public string TokenType { get; set; }
		public int ExpiresInSeconds { get; set; }
		public int ExtensionExpiresInSeconds { get; set; }
		public string AccessToken { get; set; }
	}
}