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
	public sealed class CalendarEventLocationAddressConverter : AbstractGenericJsonConverter<CalendarEventLocationAddress>
	{
		/*
		{
            "street": "",
            "city": "",
            "state": "",
            "countryOrRegion": "",
            "postalCode": ""
         }
		*/

		private const string ATTRIBUTE_STREET = "street";
		private const string ATTRIBUTE_CITY = "city";
		private const string ATTRIBUTE_STATE = "state";
		private const string ATTRIBUTE_COUNTRY_OR_REGION = "countryOrRegion";
		private const string ATTRIBUTE_POSTAL_CODE = "postalCode";

		protected override void WriteProperties(JsonWriter writer, CalendarEventLocationAddress value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.Street != null)
			{
				writer.WritePropertyName(ATTRIBUTE_STREET);
				serializer.Serialize(writer, value.Street);
			}

			if (value.City != null)
			{
				writer.WritePropertyName(ATTRIBUTE_CITY);
				serializer.Serialize(writer, value.City);
			}

			if (value.State != null)
			{
				writer.WritePropertyName(ATTRIBUTE_STATE);
				serializer.Serialize(writer, value.State);
			}

			if (value.CountryOrRegion != null)
			{
				writer.WritePropertyName(ATTRIBUTE_COUNTRY_OR_REGION);
				serializer.Serialize(writer, value.CountryOrRegion);
			}

			if (value.PostalCode != null)
			{
				writer.WritePropertyName(ATTRIBUTE_POSTAL_CODE);
				serializer.Serialize(writer, value.PostalCode);
			}
		}

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