
using System.Collections;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventAttendeesEmailConverter : AbstractGenericJsonConverter<AttendeesEmail>
	{
		private const string ATTRIBUTE_NAME = "name";
		private const string ATTRIBUTE_ADDRESS = "address";

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