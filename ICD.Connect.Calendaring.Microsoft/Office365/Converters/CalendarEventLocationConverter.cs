using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventLocationConverter : AbstractGenericJsonConverter<CalendarEventLocation>
	{
		/*
		{
            "displayName": "ConfRoom",
            "locationUri": "confroom@profoundtech.onmicrosoft.com",
            "locationType": "conferenceRoom",
            "uniqueId": "confroom@profoundtech.onmicrosoft.com",
            "uniqueIdType": "directory",
            "address": {
                "street": "",
                "city": "",
                "state": "",
                "countryOrRegion": "",
                "postalCode": ""
            },
            "coordinates": {}
        }
		*/
		 
		private const string ATTRIBUTE_DISPLAY_NAME = "displayName";
		private const string ATTRIBUTE_LOCATION_URI = "locationUri";
		private const string ATTRIBUTE_LOCATION_TYPE = "locationType";
		private const string ATTRIBUTE_UNIQUE_ID = "uniqieId";
		private const string ATTRIBUTE_UNIQUE_ID_TYPE = "uniqueIdType";
		private const string ATTRIBUTE_ADDRESS = "address";
		private const string ATTRIBUTE_COORDINATES = "coordinates";

		protected override void WriteProperties(JsonWriter writer, CalendarEventLocation value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.DisplayName != null)
			{
				writer.WritePropertyName(ATTRIBUTE_DISPLAY_NAME);
				serializer.Serialize(writer, value.DisplayName);
			}

			if (value.LocationUri != null)
			{
				writer.WritePropertyName(ATTRIBUTE_LOCATION_URI);
				serializer.Serialize(writer, value.LocationUri);
			}

			if (value.LocationType != null)
			{
				writer.WritePropertyName(ATTRIBUTE_LOCATION_TYPE);
				serializer.Serialize(writer, value.LocationType);
			}

			if (value.UniqueId != null)
			{
				writer.WritePropertyName(ATTRIBUTE_UNIQUE_ID);
				serializer.Serialize(writer, value.UniqueId);
			}

			if (value.UniqueIdType != null)
			{
				writer.WritePropertyName(ATTRIBUTE_UNIQUE_ID_TYPE);
				serializer.Serialize(writer, value.UniqueIdType);
			}

			if (value.Address != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ADDRESS);
				serializer.Serialize(writer, value.Address);
			}

			if (value.Coordinates != null)
			{
				writer.WritePropertyName(ATTRIBUTE_COORDINATES);
				serializer.Serialize(writer, value.Coordinates);
			}
		}

		protected override void ReadProperty(string property, JsonReader reader, CalendarEventLocation instance,JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_DISPLAY_NAME:
					instance.DisplayName = reader.GetValueAsString();
					break;
				case ATTRIBUTE_LOCATION_URI:
					instance.LocationUri = reader.GetValueAsString();
					break;
				case ATTRIBUTE_LOCATION_TYPE:
					instance.LocationType = reader.GetValueAsString();
					break;
				case ATTRIBUTE_UNIQUE_ID:
					instance.UniqueId = reader.GetValueAsString();
					break;
				case ATTRIBUTE_UNIQUE_ID_TYPE:
					instance.UniqueIdType = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ADDRESS:
					instance.Address = serializer.Deserialize<CalendarEventLocationAddress>(reader);
					break;
				case ATTRIBUTE_COORDINATES:
					instance.Coordinates = serializer.Deserialize<CalendarEventLocationCoordinates>(reader);
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;

			}
		}

	}
}