using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventAttendeesConverter : AbstractGenericJsonConverter<CalendarEventAttendees>
	{
		private const string ATTRIBUTE_TYPE = "type";
		private const string ATTRIBITE_STATUS = "status";
		private const string ATTRIBUTE_EMAIL_ADDRESS = "emailAddress";


		protected override void ReadProperty(string property, JsonReader reader, CalendarEventAttendees instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_TYPE:
					instance.Type = reader.GetValueAsString();
					break;
				case ATTRIBITE_STATUS:
					instance.Status = serializer.Deserialize<AttendeesStatus>(reader);
					break;
				case ATTRIBUTE_EMAIL_ADDRESS:
					instance.EmailAdress = serializer.Deserialize<AttendeesEmail>(reader);
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;

			}
		}
	}
}