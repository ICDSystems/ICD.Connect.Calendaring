using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventBodyConverter : AbstractGenericJsonConverter<CalendarEventBody>
	{
		private const string ATTRIBUTE_CONTENT_TYPE = "contentType";
		private const string ATTRIBUTE_CONTENT = "content";


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