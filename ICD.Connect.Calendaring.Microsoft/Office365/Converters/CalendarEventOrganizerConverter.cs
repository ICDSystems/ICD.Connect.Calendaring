#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventOrganizerConverter : AbstractGenericJsonConverter<CalendarEventOrganizer>
	{
		private const string ATTRIBUTE_EMAIL_ADDRESS = "emailAddress";

		protected override void WriteProperties(JsonWriter writer, CalendarEventOrganizer value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.EmailAddress != null)
			{
				writer.WritePropertyName(ATTRIBUTE_EMAIL_ADDRESS);
				serializer.Serialize(writer, value.EmailAddress);
			}
		}

		protected override void ReadProperty(string property, JsonReader reader, CalendarEventOrganizer instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_EMAIL_ADDRESS:
					instance.EmailAddress = serializer.Deserialize<CalendarEventEmailAddress>(reader);
					break;

				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}

		}
	}
}