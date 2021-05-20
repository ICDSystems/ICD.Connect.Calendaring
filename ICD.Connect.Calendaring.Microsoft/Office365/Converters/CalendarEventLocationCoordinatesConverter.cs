using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventLocationCoordinatesConverter : AbstractGenericJsonConverter<CalendarEventLocationCoordinates>
	{
		private const string ATTRIBUTE_LATITUDE = "latitude";
		private const string ATTRIBUTE_LONGITUDE = "longitude";

		protected override void WriteProperties(JsonWriter writer, CalendarEventLocationCoordinates value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.Latitude != null)
			{
				writer.WritePropertyName(ATTRIBUTE_LATITUDE);
				serializer.Serialize(writer, value.Latitude);
			}

			if (value.Longitude != null)
			{
				writer.WritePropertyName(ATTRIBUTE_LONGITUDE);
				serializer.Serialize(writer, value.Longitude);
			}
		}

		protected override void ReadProperty(string property, JsonReader reader, CalendarEventLocationCoordinates instance,
		                                     JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_LATITUDE:
					instance.Latitude = reader.GetValueAsString();
					break;
				case ATTRIBUTE_LONGITUDE:
					instance.Longitude = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}