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
	public sealed class CalendarEventAttendeesEmailConverter : AbstractGenericJsonConverter<AttendeesEmail>
	{
		private const string ATTRIBUTE_NAME = "name";
		private const string ATTRIBUTE_ADDRESS = "address";

		protected override void WriteProperties(JsonWriter writer, AttendeesEmail value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.Name != null)
			{
				writer.WritePropertyName(ATTRIBUTE_NAME);
				serializer.Serialize(writer, value.Name);
			}

			if (value.Address != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ADDRESS);
				serializer.Serialize(writer, value.Address);
			}
		}

		protected override void ReadProperty(string property, JsonReader reader, AttendeesEmail instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_NAME:
					instance.Name = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ADDRESS:
					instance.Address = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}



	}
}