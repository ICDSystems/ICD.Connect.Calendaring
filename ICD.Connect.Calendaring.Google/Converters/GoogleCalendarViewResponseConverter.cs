using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public sealed class GoogleCalendarViewResponseConverter : GoogleAbstractResponseConverter<GoogleCalendarViewResponse>
	{
		private const string ATTRIBUTE_KIND = "kind";
		private const string ATTRIBUTE_E_TAG = "etag";
		private const string ATTRIBUTE_SUMMARY = "summary";
		private const string ATTRIBUTE_UPDATED = "updated";
		private const string ATTRIBUTE_TIMEZONE = "timeZone";
		private const string ATTRIBUTE_ACCESS_ROLE = "accessRole";
		private const string ATTRIBUTE_DEFAULT_REMINDERS = "defaultReminders";
		private const string ATTRIBUTE_NEXT_SYNC_TOKEN = "nextSyncToken";
		private const string ATTRIBUTE_ITEMS = "items";


		protected override void ReadProperty(string property, JsonReader reader, GoogleCalendarViewResponse instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_KIND:
					instance.Kind = reader.GetValueAsString();
					break;
				case ATTRIBUTE_E_TAG:
					instance.ETag = reader.GetValueAsString();
					break;
				case ATTRIBUTE_SUMMARY:
					instance.Summary = reader.GetValueAsString();
					break;
				case ATTRIBUTE_UPDATED:
					instance.Updated = reader.GetValueAsDateTime();
					break;
				case ATTRIBUTE_TIMEZONE:
					instance.TimeZone = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ACCESS_ROLE:
					instance.AccessRole = reader.GetValueAsString();
					break;
				case ATTRIBUTE_DEFAULT_REMINDERS:
					instance.DefaultReminders = serializer.DeserializeArray<GoogleDefaultReminder>(reader).ToArray();
					break;
				case ATTRIBUTE_NEXT_SYNC_TOKEN:
					instance.NextSyncToken = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ITEMS:
					instance.Items = serializer.DeserializeArray<GoogleCalendarEvent>(reader).ToArray();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}
