#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Google.Responses;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public sealed class GoogleCalendarEventEmailConverter : AbstractGenericJsonConverter<GoogleCalendarEventEmail>
	{
		private const string ATTRIBUTE_EMAIL = "email";

		protected override void WriteProperties(JsonWriter writer, GoogleCalendarEventEmail value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.Email != null)
			{
				writer.WritePropertyName(ATTRIBUTE_EMAIL);
				serializer.Serialize(writer, value.Email);
			}
		}

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
