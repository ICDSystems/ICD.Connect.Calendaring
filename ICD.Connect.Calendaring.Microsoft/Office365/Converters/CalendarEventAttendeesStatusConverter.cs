#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventAttendeesStatusConverter : AbstractGenericJsonConverter<AttendeesStatus>
	{
		private const string ATTRIBUTE_RESPONSE = "response";
		private const string ATTRIBUTE_TIME = "time";

		protected override void WriteProperties(JsonWriter writer, AttendeesStatus value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.Response != null)
			{
				writer.WritePropertyName(ATTRIBUTE_RESPONSE);
				serializer.Serialize(writer, value.Response);
			}

			writer.WritePropertyName(ATTRIBUTE_TIME);
			serializer.Serialize(writer, value.Time);
		}

		protected override void ReadProperty(string property, JsonReader reader, AttendeesStatus instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_RESPONSE:
					instance.Response = reader.GetValueAsString();
					break;
				case ATTRIBUTE_TIME:
					instance.Time = reader.GetValueAsDateTime().ToUniversalTime();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}