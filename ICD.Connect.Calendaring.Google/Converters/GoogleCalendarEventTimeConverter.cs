using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public sealed class GoogleCalendarEventTimeConverter : AbstractGenericJsonConverter<GoogleCalendarEventTime>
	{
		private const string ATTRIBUTE_DATE_TIME = "dateTime";

		protected override void ReadProperty(string property, JsonReader reader, GoogleCalendarEventTime instance,JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_DATE_TIME:
					instance.DateTime = reader.GetValueAsDateTime();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}

		}
	}
}
