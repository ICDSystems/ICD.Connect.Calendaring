
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventLocationAddressConverter : AbstractGenericJsonConverter<CalendarEventLocationAddress>
	{
		private const string ATTRIBUTE_STREET = "street";
		private const string ATTRIBUTE_CITY = "city";
		private const string ATTRIBUTE_STATE = "state";
		private const string ATTRIBUTE_COUNTRY_OR_REGION = "countryOrRegion";
		private const string ATTRIBUTE_POSTAL_CODE = "postalCode";



		protected override void ReadProperty(string property, JsonReader reader, CalendarEventLocationAddress instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_STREET:
					instance.Street = reader.GetValueAsString();
					break;
				case ATTRIBUTE_CITY:
					instance.City = reader.GetValueAsString();
					break;
				case ATTRIBUTE_STATE:
					instance.State = reader.GetValueAsString();
					break;
				case ATTRIBUTE_COUNTRY_OR_REGION:
					instance.CountryOrRegion = reader.GetValueAsString();
					break;
				case ATTRIBUTE_POSTAL_CODE:
					instance.PostalCode = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}

	}


}