using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventLocationConverter : AbstractGenericJsonConverter<CalendarEventLocation>
	{
		private const string ATTRIBUTE_DISPLAY_NAME = "displayName";
		private const string ATTRIBUTE_LOCATION_TYPE = "locationType";
		private const string ATTRIBUTE_UNIQUE_ID = "uniqieId";
		private const string ATTRIBUTE_UNIQUE_ID_TYPE = "uniqueIdType";
		private const string ATTRIBUTE_ADDRESS = "address";
		private const string ATTRIBUTE_COORDINATES = "coordinates";

		protected override void ReadProperty(string property, JsonReader reader, CalendarEventLocation instance,JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_DISPLAY_NAME:
					instance.DisplayName = reader.GetValueAsString();
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
					instance.Coordinates = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;

			}
		}

	}
}