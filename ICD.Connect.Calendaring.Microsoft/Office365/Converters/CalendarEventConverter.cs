#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventConverter : AbstractGenericJsonConverter<CalendarEvent>
	{
		/*
		{
            "@odata.etag": "W/\"F/P2k9S3HEuT8uqtl18ZkAAACQsHDg==\"",
            "id": "AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8-aT1LccS5Py6q2XXxmQAAAAAAENAAAX8-aT1LccS5Py6q2XXxmQAAAJDkbRAAA=",
            "createdDateTime": "2019-06-25T20:50:54.4848023Z",
            "lastModifiedDateTime": "2019-06-25T20:50:54.8096139Z",
            "changeKey": "F/P2k9S3HEuT8uqtl18ZkAAACQsHDg==",
            "categories": [],
            "originalStartTimeZone": "UTC",
            "originalEndTimeZone": "UTC",
            "iCalUId": "040000008200E00074C5B7101A82E00800000000B7797EAE972BD501000000000000000010000000F433F8266AA6B3469E48B01A606EF0C5",
            "reminderMinutesBeforeStart": 15,
            "isReminderOn": false,
            "hasAttachments": false,
            "subject": "alright",
            "bodyPreview": "",
            "importance": "normal",
            "sensitivity": "normal",
            "isAllDay": true,
            "isCancelled": false,
            "isOrganizer": true,
            "responseRequested": true,
            "seriesMasterId": null,
            "showAs": "free",
            "type": "singleInstance",
            "webLink": "https://outlook.office365.com/owa/?itemid=AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8%2FaT1LccS5Py6q2XXxmQAAAAAAENAAAX8%2FaT1LccS5Py6q2XXxmQAAAJDkbRAAA%3D&exvsurl=1&path=/calendar/item",
            "onlineMeetingUrl": null,
            "recurrence": null,
            "responseStatus": {
                "response": "organizer",
                "time": "0001-01-01T00:00:00Z"
            },
            "body": {
                "contentType": "html",
                "content": "<html>\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n<meta content=\"text/html; charset=us-ascii\">\r\n<style type=\"text/css\" style=\"display:none\">\r\n<!--\r\np\r\n\t{margin-top:0;\r\n\tmargin-bottom:0}\r\n-->\r\n</style>\r\n</head>\r\n<body dir=\"ltr\">\r\n<div id=\"divtagdefaultwrapper\" dir=\"ltr\" style=\"font-size:12pt; color:#000000; font-family:Calibri,Helvetica,sans-serif\">\r\n<p style=\"margin-top:0; margin-bottom:0\"><br>\r\n</p>\r\n</div>\r\n</body>\r\n</html>\r\n"
            },
            "start": {
                "dateTime": "2019-06-25T00:00:00.0000000",
                "timeZone": "UTC"
            },
            "end": {
                "dateTime": "2019-06-26T00:00:00.0000000",
                "timeZone": "UTC"
            },
            "location": {
                "displayName": "room",
                "locationType": "default",
                "uniqueId": "room",
                "uniqueIdType": "private"
            },
            "locations": [
                {
                    "displayName": "room",
                    "locationType": "default",
                    "uniqueId": "room",
                    "uniqueIdType": "private"
                }
            ],
            "attendees": [],
            "organizer": {
                "emailAddress": {
                    "name": "Interns",
                    "address": "interns@profoundtech.onmicrosoft.com"
                }
            }
        }
		 */

		private const string ATTRIBUTE_DATA_ETAG = "@odata.etag";
		private const string ATTRIBUTE_ID = "id";
		private const string ATTRIBUTE_CREATED_DATE_TIME = "createdDateTime";
		private const string ATTRIBUTE_LAST_MODIFIED_DATE_TIME = "lastModifiedDateTime";
		private const string ATTRIBUTE_CHANGE_KEY = "changeKey";
		private const string ATTRIBUTE_CATEGORIES = "categories";
		private const string ATTRIBUTE_ORIGINAL_START_TIME_ZONE = "originalStartTimeZone";
		private const string ATTRIBUTE_ORIGINAL_END_TIME_ZONE = "originalEndTimeZone";
		private const string ATTRIBUTE_I_CAL_U_ID = "iCaluId";
		private const string ATTRIBUTE_REMINDER_MINUTES_BEFORE_START = "reminderMinutesBeforeStart";
		private const string ATTRIBUTE_IS_REMINDER_ON = "isReminderOn";
		private const string ATTRIBUTE_HAS_ATTACHMENTS = "hasAttachments";
		private const string ATTRIBUTE_SUBJECT = "subject";
		private const string ATTRIBUTE_BODY_PREVIEW = "bodyPreview";
		private const string ATTRIBUTE_IMPORTANCE = "inportance";
		private const string ATTRIBUTE_SENSITIVITY = "sensitivity";
		private const string ATTRIBUTE_IS_ALL_DAY = "iaAllDay";
		private const string ATTRIBUTE_IS_CANCELED = "isCancelled";
		private const string ATTRIBUTE_IS_ORGANIZER = "isOrganizer";
		private const string ATTRIBUTE_RESPONSE_REQUESTED = "responseResquested";
		private const string ATTRIBUTE_SERIES_MASTER_ID = "seriesMasterId";
		private const string ATTRIBUTE_SHOW_AS = "showAs";
		private const string ATTRIBUTE_TYPE = "type";
		private const string ATTRIBUTE_WEBLINK = "webLink";
		private const string ATTRIBUTE_ONLINE_MEETING_URL = "onlineMeetingUrl";
		private const string ATTRIBUTE_RECURRENCE = "recurrence";
		private const string ATTRIBUTE_RESPONSE_STATUS = "responseStatus";
		private const string ATTRIBUTE_BODY = "body";
		private const string ATTRIBUTE_START = "start";
		private const string ATTRIBUTE_END = "end";
		private const string ATTRIBUTE_LOCATION = "location";
		private const string ATTRIBUTE_LOCATIONS = "locations";
		private const string ATTRIBITES_ATTENDEES = "attendees";
		private const string ATTRIBUTE_ORGANIZER = "organizer";

		protected override void WriteProperties(JsonWriter writer, CalendarEvent value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.ODataEtag != null)
			{
				writer.WritePropertyName(ATTRIBUTE_DATA_ETAG);
				serializer.Serialize(writer, value.ODataEtag);
			}

			if (value.Id != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ID);
				serializer.Serialize(writer, value.Id);
			}

			writer.WritePropertyName(ATTRIBUTE_CREATED_DATE_TIME);
			serializer.Serialize(writer, value.CreatedDateTime);

			writer.WritePropertyName(ATTRIBUTE_LAST_MODIFIED_DATE_TIME);
			serializer.Serialize(writer, value.LastModifiedDateTime);

			if (value.ChangeKey != null)
			{
				writer.WritePropertyName(ATTRIBUTE_CHANGE_KEY);
				serializer.Serialize(writer, value.ChangeKey);
			}

			if (value.Categories != null)
			{
				writer.WritePropertyName(ATTRIBUTE_CATEGORIES);
				serializer.SerializeArray(writer, value.Categories);
			}

			if (value.OriginalStartTimeZone != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ORIGINAL_START_TIME_ZONE);
				serializer.Serialize(writer, value.OriginalStartTimeZone);
			}

			if (value.OriginalEndTimeZone != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ORIGINAL_END_TIME_ZONE);
				serializer.Serialize(writer, value.OriginalEndTimeZone);
			}

			if (value.ICalUId != null)
			{
				writer.WritePropertyName(ATTRIBUTE_I_CAL_U_ID);
				serializer.Serialize(writer, value.ICalUId);
			}

			if (value.ReminderMinutesBeforeStart != 0)
			{
				writer.WritePropertyName(ATTRIBUTE_REMINDER_MINUTES_BEFORE_START);
				serializer.Serialize(writer, value.ReminderMinutesBeforeStart);
			}

			if (value.IsReminderOn)
			{
				writer.WritePropertyName(ATTRIBUTE_IS_REMINDER_ON);
				serializer.Serialize(writer, value.IsReminderOn);
			}

			if (value.HasAttachments)
			{
				writer.WritePropertyName(ATTRIBUTE_HAS_ATTACHMENTS);
				serializer.Serialize(writer, value.HasAttachments);
			}

			if (value.Subject != null)
			{
				writer.WritePropertyName(ATTRIBUTE_SUBJECT);
				serializer.Serialize(writer, value.Subject);
			}

			if (value.BodyPreview != null)
			{
				writer.WritePropertyName(ATTRIBUTE_BODY_PREVIEW);
				serializer.Serialize(writer, value.BodyPreview);
			}

			if (value.Importance != null)
			{
				writer.WritePropertyName(ATTRIBUTE_IMPORTANCE);
				serializer.Serialize(writer, value.Importance);
			}

			if (value.Sensitivity != null)
			{
				writer.WritePropertyName(ATTRIBUTE_SENSITIVITY);
				serializer.Serialize(writer, value.Sensitivity);
			}

			if (value.IsAllDay)
			{
				writer.WritePropertyName(ATTRIBUTE_IS_ALL_DAY);
				serializer.Serialize(writer, value.IsAllDay);
			}

			if (value.IsCancelled)
			{
				writer.WritePropertyName(ATTRIBUTE_IS_CANCELED);
				serializer.Serialize(writer, value.IsCancelled);
			}

			if (value.IsOrganizer)
			{
				writer.WritePropertyName(ATTRIBUTE_IS_ORGANIZER);
				serializer.Serialize(writer, value.IsOrganizer);
			}

			if (value.ResponseRequested)
			{
				writer.WritePropertyName(ATTRIBUTE_RESPONSE_REQUESTED);
				serializer.Serialize(writer, value.ResponseRequested);
			}

			if (value.SeriesMasterId != null)
			{
				writer.WritePropertyName(ATTRIBUTE_SERIES_MASTER_ID);
				serializer.Serialize(writer, value.SeriesMasterId);
			}

			if (value.ShowAs != null)
			{
				writer.WritePropertyName(ATTRIBUTE_SHOW_AS);
				serializer.Serialize(writer, value.ShowAs);
			}

			if (value.Type != null)
			{
				writer.WritePropertyName(ATTRIBUTE_TYPE);
				serializer.Serialize(writer, value.Type);
			}

			if (value.WebLink != null)
			{
				writer.WritePropertyName(ATTRIBUTE_WEBLINK);
				serializer.Serialize(writer, value.WebLink);
			}

			if (value.OnlineMeetingUrl != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ONLINE_MEETING_URL);
				serializer.Serialize(writer, value.OnlineMeetingUrl);
			}

			if (value.Recurence != null)
			{
				writer.WritePropertyName(ATTRIBUTE_RECURRENCE);
				serializer.Serialize(writer, value.Recurence);
			}

			if (value.ResponseStatus != null)
			{
				writer.WritePropertyName(ATTRIBUTE_RESPONSE_STATUS);
				serializer.Serialize(writer, value.ResponseStatus);
			}

			if (value.Body != null)
			{
				writer.WritePropertyName(ATTRIBUTE_BODY);
				serializer.Serialize(writer, value.Body);
			}

			if (value.Start != null)
			{
				writer.WritePropertyName(ATTRIBUTE_START);
				serializer.Serialize(writer, value.Start);
			}

			if (value.End != null)
			{
				writer.WritePropertyName(ATTRIBUTE_END);
				serializer.Serialize(writer, value.End);
			}

			if (value.Location != null)
			{
				writer.WritePropertyName(ATTRIBUTE_LOCATION);
				serializer.Serialize(writer, value.Location);
			}

			if (value.Locations != null)
			{
				writer.WritePropertyName(ATTRIBUTE_LOCATIONS);
				serializer.SerializeArray(writer, value.Locations);
			}

			if (value.Attendees != null)
			{
				writer.WritePropertyName(ATTRIBITES_ATTENDEES);
				serializer.SerializeArray(writer, value.Attendees);
			}

			if (value.Organizer != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ORGANIZER);
				serializer.Serialize(writer, value.Organizer);
			}
		}

		protected override void ReadProperty(string property, JsonReader reader, CalendarEvent instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_DATA_ETAG:
					instance.ODataEtag = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ID:
					instance.Id = reader.GetValueAsString();
					break;
				case ATTRIBUTE_CREATED_DATE_TIME:
					instance.CreatedDateTime = reader.GetValueAsDateTime().ToUniversalTime();
					break;
				case ATTRIBUTE_LAST_MODIFIED_DATE_TIME:
					instance.LastModifiedDateTime = reader.GetValueAsDateTime().ToUniversalTime();
					break;
				case ATTRIBUTE_CHANGE_KEY:
					instance.ChangeKey = reader.GetValueAsString();
					break;
				case ATTRIBUTE_CATEGORIES:
					instance.Categories = serializer.DeserializeArray<string>(reader).ToArray();
					break;
				case ATTRIBUTE_ORIGINAL_START_TIME_ZONE:
					instance.OriginalStartTimeZone = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ORIGINAL_END_TIME_ZONE:
					instance.OriginalEndTimeZone = reader.GetValueAsString();
					break;
				case ATTRIBUTE_I_CAL_U_ID:
					instance.ICalUId = reader.GetValueAsString();
					break;
				case ATTRIBUTE_REMINDER_MINUTES_BEFORE_START:
					instance.ReminderMinutesBeforeStart = reader.GetValueAsInt();
					break;
				case ATTRIBUTE_IS_REMINDER_ON:
					instance.IsReminderOn = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_HAS_ATTACHMENTS:
					instance.HasAttachments = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_SUBJECT:
					instance.Subject = reader.GetValueAsString();
					break;
				case ATTRIBUTE_BODY_PREVIEW:
					instance.BodyPreview = reader.GetValueAsString();
					break;
				case ATTRIBUTE_IMPORTANCE:
					instance.Importance = reader.GetValueAsString();
					break;
				case ATTRIBUTE_SENSITIVITY:
					instance.Sensitivity = reader.GetValueAsString();
					break;
				case ATTRIBUTE_IS_ALL_DAY:
					instance.IsAllDay = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_IS_CANCELED:
					instance.IsCancelled = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_IS_ORGANIZER:
					instance.IsOrganizer = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_RESPONSE_REQUESTED:
					instance.ResponseRequested = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_SERIES_MASTER_ID:
					instance.SeriesMasterId = reader.GetValueAsString();
					break;
				case ATTRIBUTE_SHOW_AS:
					instance.ShowAs = reader.GetValueAsString();
					break;
				case ATTRIBUTE_TYPE:
					instance.Type = reader.GetValueAsString();
					break;
				case ATTRIBUTE_WEBLINK:
					instance.WebLink = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ONLINE_MEETING_URL:
					instance.OnlineMeetingUrl = reader.GetValueAsString();
					break;
				case ATTRIBUTE_RECURRENCE:
					instance.Recurence = reader.GetValueAsString();
					break;
				case ATTRIBUTE_RESPONSE_STATUS:
					instance.ResponseStatus = serializer.Deserialize<CalendarEventResponseStatus>(reader);
					break;
				case ATTRIBUTE_BODY:
					instance.Body = serializer.Deserialize<CalendarEventBody>(reader);
					break;
				case ATTRIBUTE_START:
					instance.Start = serializer.Deserialize<CalendarEventTime>(reader);
					break;
				case ATTRIBUTE_END:
					instance.End = serializer.Deserialize<CalendarEventTime>(reader);
					break;
				case ATTRIBUTE_LOCATION:
					instance.Location = serializer.Deserialize<CalendarEventLocation>(reader);
					break;
				case ATTRIBUTE_LOCATIONS:
					instance.Locations = serializer.DeserializeArray<CalendarEventLocation>(reader).ToArray();
					break;
				case ATTRIBITES_ATTENDEES:
					instance.Attendees = serializer.DeserializeArray<CalendarEventAttendees>(reader).ToArray();
					break;
				case ATTRIBUTE_ORGANIZER:
					instance.Organizer = serializer.Deserialize<CalendarEventOrganizer>(reader);
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;


			}


		}
	}
}