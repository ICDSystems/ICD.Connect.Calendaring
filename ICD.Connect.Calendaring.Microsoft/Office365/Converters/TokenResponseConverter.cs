using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class TokenResponseConverter : AbstractGenericJsonConverter<TokenResponse>
	{
		/*
		{
			"token_type": "Bearer",
			"expires_in": 3600,
			"ext_expires_in": 3600,
			"access_token": "eyJ0eXAiOiJKV1QiLCJub25jZSI6IkFRQUJBQUFBQUFEQ29NcGpKWHJ4VHE5Vkc5dGUtN0ZYVWE4Mlp0RFZSM1d6bkhPOWtHVGlPQS0yaWlPd1pYVHRJZGUwLXFUSkhJVy02RzFLQTBYYUZRQm54YWJ1ZjJyTnYwMzdGYk1TLXRnbzRZRUZlYVA0NGlBQSIsImFsZyI6IlJTMjU2IiwieDV0IjoiQ3RmUUM4TGUtOE5zQzdvQzJ6UWtacGNyZk9jIiwia2lkIjoiQ3RmUUM4TGUtOE5zQzdvQzJ6UWtacGNyZk9jIn0.eyJhdWQiOiJodHRwczovL2dyYXBoLm1pY3Jvc29mdC5jb20iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC82NzRjYTI5NS04ZmVjLTQzYTItOTU3Yy0wZDRlNmYyZTE2YmIvIiwiaWF0IjoxNTYxNDcxNzc1LCJuYmYiOjE1NjE0NzE3NzUsImV4cCI6MTU2MTQ3NTY3NSwiYWlvIjoiNDJaZ1lQZzQ4VjFtZzdQVHJIVmM0UnNiTjh4c0JRQT0iLCJhcHBfZGlzcGxheW5hbWUiOiJMYXN0VGVzdCIsImFwcGlkIjoiYmZkMDVmNjYtZDY5MS00YWY1LThkMjctNmNkOTkwOTQ5OTllIiwiYXBwaWRhY3IiOiIxIiwiaWRwIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvNjc0Y2EyOTUtOGZlYy00M2EyLTk1N2MtMGQ0ZTZmMmUxNmJiLyIsIm9pZCI6IjNiZTgyZTliLWRhMzYtNGNmOC05YTJhLTI0ZTYzNGZkYTc4ZSIsInJvbGVzIjpbIkNhbGVuZGFycy5SZWFkIiwiQ2FsZW5kYXJzLlJlYWRXcml0ZSJdLCJzdWIiOiIzYmU4MmU5Yi1kYTM2LTRjZjgtOWEyYS0yNGU2MzRmZGE3OGUiLCJ0aWQiOiI2NzRjYTI5NS04ZmVjLTQzYTItOTU3Yy0wZDRlNmYyZTE2YmIiLCJ1dGkiOiJ3QlR1ZnFJSm9FbWZkOWFYbkI0WEFBIiwidmVyIjoiMS4wIiwieG1zX3RjZHQiOjE1MDE3OTczMDZ9.LJmvC4q11jviTSLBVP9VfwXT3cysS08QSBQhDb_UuhkmW8NZ3FqQc54scnFj3I8v2wSdHToQb__uDP_IolPxPJz6sUI4sSJJp_yXKpbDJj6v-6-HeYV0bNyrsu8W4x0nCnqu_0-kuhpYiZJ_ofEx01G5Q8aqCERokBy1AL8kYgPxc1i7uesWWEqS2J0qVj-e9zNm9Go3guKQZ3q8dN60WPPuFkxr3zOTAG3fk10Ka6_BSFsPau28ylSqDZkdF7I0_Dp-veZ6Rs6ICm98Js-Hq7sq99W9kmiM2_6f17-7BS_2nCqIy-9CtjYbWTfOq_8Ii527bBHDd1VCb79XFQw3sQ"
		}
		 */

		private const string TOKE_TYPE = "token_type";
		private const string EXPIRES_IN = "expires_in";
		private const string EXT_EXPIRES_IN = "ext_expires_in";
		private const string ACCESS_TOKEN = "access_token";
	
		/// <summary>
		/// Override to handle the current property value with the given name.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="reader"></param>
		/// <param name="instance"></param>
		/// <param name="serializer"></param>
		protected override void ReadProperty(string property, JsonReader reader, TokenResponse instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case TOKE_TYPE:
					instance.TokenType = reader.GetValueAsString();
					break;
				case EXPIRES_IN:
					instance.ExpiresInSeconds = reader.GetValueAsInt();
					break;
				case EXT_EXPIRES_IN:
					instance.ExtensionExpiresInSeconds = reader.GetValueAsInt();
					break;
				case ACCESS_TOKEN:
					instance.AccessToken = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}