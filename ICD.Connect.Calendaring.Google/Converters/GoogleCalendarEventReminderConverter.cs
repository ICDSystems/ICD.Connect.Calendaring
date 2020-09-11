using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public sealed class GoogleCalendarEventReminderConverter : AbstractGenericJsonConverter<GoogleCalendarEventReminder>
	{
		private const string ATTRIBUTE_USE_DEFAULT = "useDefault";

		protected override void WriteProperties(JsonWriter writer, GoogleCalendarEventReminder value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.UseDefault)
			{
				writer.WritePropertyName(ATTRIBUTE_USE_DEFAULT);
				serializer.Serialize(writer, value.UseDefault);
			}
		}

		protected override void ReadProperty(string property, JsonReader reader, GoogleCalendarEventReminder instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_USE_DEFAULT:
					instance.UseDefault = reader.GetValueAsBool();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}
