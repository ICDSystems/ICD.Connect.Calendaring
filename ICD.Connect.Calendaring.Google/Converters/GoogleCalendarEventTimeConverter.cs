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
	public sealed class GoogleCalendarEventTimeConverter : AbstractGenericJsonConverter<GoogleCalendarEventTime>
	{
		private const string ATTRIBUTE_DATE_TIME = "dateTime";

		protected override void WriteProperties(JsonWriter writer, GoogleCalendarEventTime value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			writer.WritePropertyName(ATTRIBUTE_DATE_TIME);
			serializer.Serialize(writer, value.DateTime);
		}

		protected override void ReadProperty(string property, JsonReader reader, GoogleCalendarEventTime instance,JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_DATE_TIME:
					instance.DateTime = reader.GetValueAsDateTime().ToUniversalTime();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}

		}
	}
}
