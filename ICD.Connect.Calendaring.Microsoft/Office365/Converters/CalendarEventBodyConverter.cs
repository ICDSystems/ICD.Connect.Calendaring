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
	public sealed class CalendarEventBodyConverter : AbstractGenericJsonConverter<CalendarEventBody>
	{
		private const string ATTRIBUTE_CONTENT_TYPE = "contentType";
		private const string ATTRIBUTE_CONTENT = "content";

		protected override void WriteProperties(JsonWriter writer, CalendarEventBody value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.ContentType != null)
			{
				writer.WritePropertyName(ATTRIBUTE_CONTENT_TYPE);
				serializer.Serialize(writer, value.ContentType);
			}

			if (value.Content != null)
			{
				writer.WritePropertyName(ATTRIBUTE_CONTENT);
				serializer.Serialize(writer, value.Content);
			}
		}

		protected override void ReadProperty(string property, JsonReader reader, CalendarEventBody instance,JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_CONTENT_TYPE:
					instance.ContentType = reader.GetValueAsString();
					break;
				case ATTRIBUTE_CONTENT:
					instance.Content = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;

			}
			
		}

	}
}