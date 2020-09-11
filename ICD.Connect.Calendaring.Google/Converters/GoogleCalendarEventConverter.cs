using System;
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

		protected override void WriteProperties(JsonWriter writer, GoogleCalendarEvent value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.Kind != null)
			{
				writer.WritePropertyName(ATTRIBUTE_KIND);
				serializer.Serialize(writer, value.Kind);
			}

			if (value.ETag != null)
			{
				writer.WritePropertyName(ATTRIBUTE_E_TAG);
				serializer.Serialize(writer, value.ETag);
			}

			if (value.Id != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ID);
				serializer.Serialize(writer, value.Id);
			}

			if (value.Status != null)
			{
				writer.WritePropertyName(ATTRIBUTE_STATUS);
				serializer.Serialize(writer, value.Status);
			}

			if (value.HtmlLink != null)
			{
				writer.WritePropertyName(ATTRIBUTE_HTML_LINK);
				serializer.Serialize(writer, value.HtmlLink);
			}

			if (value.Created != DateTime.MinValue)
			{
				writer.WritePropertyName(ATTRIBUTE_CREATED);
				serializer.Serialize(writer, value.Created);
			}

			if (value.Updated != DateTime.MinValue)
			{
				writer.WritePropertyName(ATTRIBUTE_UPDATED);
				serializer.Serialize(writer, value.Updated);
			}

			if (value.Summary != null)
			{
				writer.WritePropertyName(ATTRIBUTE_SUMMARY);
				serializer.Serialize(writer, value.Summary);
			}

			if (value.Description != null)
			{
				writer.WritePropertyName(ATTRIBUTE_DESCRIPTION);
				serializer.Serialize(writer, value.Description);
			}

			if (value.Location != null)
			{
				writer.WritePropertyName(ATTRIBUTE_LOCATION);
				serializer.Serialize(writer, value.Location);
			}

			if (value.Creator != null)
			{
				writer.WritePropertyName(ATTRIBUTE_CREATOR);
				serializer.Serialize(writer, value.Creator);
			}

			if (value.Organizer != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ORGANIZER);
				serializer.Serialize(writer, value.Organizer);
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

			if (value.ICalUid != null)
			{
				writer.WritePropertyName(ATTRIBUTE_I_CAL_UID);
				serializer.Serialize(writer, value.ICalUid);
			}

			if (value.Sequence != null)
			{
				writer.WritePropertyName(ATTRIBUTE_SEQUENCE);
				serializer.Serialize(writer, value.Sequence);
			}

			if (value.Attendees != null)
			{
				writer.WritePropertyName(ATTRIBUTE_ATTENDEES);
				serializer.SerializeArray(writer, value.Attendees);
			}

			if (value.GuestCanModify)
			{
				writer.WritePropertyName(ATTRIBUTE_GUEST_CAN_MODIFY);
				serializer.Serialize(writer, value.GuestCanModify);
			}

			if (value.Reminders != null)
			{
				writer.WritePropertyName(ATTRIBUTE_REMINDERS);
				serializer.Serialize(writer, value.Reminders);
			}
		}

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
					instance.Created = reader.GetValueAsDateTime().ToUniversalTime();
					break;
				case ATTRIBUTE_UPDATED:
					instance.Updated = reader.GetValueAsDateTime().ToUniversalTime();
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
