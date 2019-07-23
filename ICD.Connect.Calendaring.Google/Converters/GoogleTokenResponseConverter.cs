using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public sealed class GoogleTokenResponseConverter: GoogleAbstractResponseConverter<GoogleTokenResponse>
	{
		private const string ATTRIBUTE_ACCESS_TOKEN = "access_token";
		private const string ATTRIBUTE_EXPIRES_ON = "expires_in";
		private const string ATTRIBUTE_TOKEN_TYPE = "token_type";

		protected override void ReadProperty(string property, JsonReader reader, GoogleTokenResponse instance,
		                                   JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_ACCESS_TOKEN:
					instance.AccessToken = reader.GetValueAsString();
					break;
				case ATTRIBUTE_EXPIRES_ON:
					instance.ExpiresInSeconds = reader.GetValueAsInt();
					break;
				case ATTRIBUTE_TOKEN_TYPE:
					instance.TokenType = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;

			}

		}
	}
}
