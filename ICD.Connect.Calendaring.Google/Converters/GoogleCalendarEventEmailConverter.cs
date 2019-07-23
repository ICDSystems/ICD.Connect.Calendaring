using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public sealed class GoogleCalendarEventEmailConverter : AbstractGenericJsonConverter<GoogleCalendarEventEmail>
	{
		private const string ATTRIBUTE_EMAIL = "email";

		protected override void ReadProperty(string property, JsonReader reader, GoogleCalendarEventEmail instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_EMAIL:
					instance.Email = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}
