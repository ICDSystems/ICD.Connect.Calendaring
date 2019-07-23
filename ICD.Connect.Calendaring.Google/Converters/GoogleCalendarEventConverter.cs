using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;
using System.Linq;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public sealed class GoogleCalendarEventConverter : AbstractGenericJsonConverter<GoogleCalendarEvent>
	{
		private const string ATTRIBUTE_KIND = "kind";
		private const string ATTRIBUTE_E_TAG = "etag";
		private const string ATTRIBUTE_ID = "id";
		private const string ATTRIBUTE_STATUS = "status";
		private const string ATTRIBUTE_HTML_LINK = "htmlLink";
		private const string ATTRIBUTE_CREATED = "created";
		private const string ATTRIBUTE_UPDATED = "updated";
		private const string ATTRIBUTE_SUMMARY = "summary";
		private const string ATTRIBUTE_DESCRIPTION = "description";
		private const string ATTRIBUTE_LOCATION = "location";
		private const string ATTRIBUTE_CREATOR = "creator";
		private const string ATTRIBUTE_ORGANIZER = "organizer";
		private const string ATTRIBUTE_START = "start";
		private const string ATTRIBUTE_END = "end";
		private const string ATTRIBUTE_I_CAL_UID = "iCalUID";
		private const string ATTRIBUTE_SEQUENCE = "sequence";
		private const string ATTRIBUTE_ATTENDEES = "attendees";
		private const string ATTRIBUTE_GUEST_CAN_MODIFY = "guestCanModify";
		private const string ATTRIBUTE_REMINDERS = "reminders";

		protected override void ReadProperty(string property, JsonReader reader, GoogleCalendarEvent instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_KIND:
					instance.Kind = reader.GetValueAsString();
					break;
				case ATTRIBUTE_E_TAG:
					instance.ETag = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ID:
					instance.Id = reader.GetValueAsString();
					break;
				case ATTRIBUTE_STATUS:
					instance.Status = reader.GetValueAsString();
					break;
				case ATTRIBUTE_HTML_LINK:
					instance.HtmlLink = reader.GetValueAsString();
					break;
				case ATTRIBUTE_CREATED:
					instance.Created = reader.GetValueAsDateTime();
					break;
				case ATTRIBUTE_UPDATED:
					instance.Updated = reader.GetValueAsDateTime();
					break;
				case ATTRIBUTE_SUMMARY:
					instance.Summary = reader.GetValueAsString();
					break;
				case ATTRIBUTE_DESCRIPTION:
					instance.Description = reader.GetValueAsString();
					break;
				case ATTRIBUTE_LOCATION:
					instance.Location = reader.GetValueAsString();
					break;
				case ATTRIBUTE_CREATOR:
					instance.Creator = serializer.Deserialize<GoogleCalendarEventEmail>(reader);
					break;
				case ATTRIBUTE_ORGANIZER:
					instance.Organizer = serializer.Deserialize<GoogleCalendarEventEmail>(reader);
					break;
				case ATTRIBUTE_START:
					instance.Start = serializer.Deserialize<GoogleCalendarEventTime>(reader);
					break;
				case ATTRIBUTE_END:
					instance.End = serializer.Deserialize<GoogleCalendarEventTime>(reader);
					break;
				case ATTRIBUTE_I_CAL_UID:
					instance.ICalUid = reader.GetValueAsString();
					break;
				case ATTRIBUTE_SEQUENCE:
					instance.Sequence = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ATTENDEES:
					instance.Attendees = serializer.DeserializeArray<GoogleCalendarEventAttendee>(reader).ToArray();
					break;
				case ATTRIBUTE_GUEST_CAN_MODIFY:
					instance.GuestCanModify = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_REMINDERS:
					instance.Reminders = serializer.Deserialize<GoogleCalendarEventReminder>(reader);
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}

		}
	}
}
