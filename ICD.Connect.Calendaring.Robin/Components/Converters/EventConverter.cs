using System;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Robin.Components.Events;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Converters;

namespace ICD.Connect.Calendaring.Robin.Components.Converters
{
	public sealed class EventConverter : AbstractGenericJsonConverter<Event>
	{
		private const string ATTR_TITLE = "title";
		private const string ATTR_START = "start";
		private const string ATTR_END = "end";
		private const string ATTR_CREATOR_ID = "creator_id";
		private const string ATTR_CREATOR_EMAIL = "creator_email";
		private const string ATTR_ID = "id";
		private const string ATTR_DESCRIPTION = "description";
		private const string ATTR_VISIBILITY = "visibility";
		private const string ATTR_INVITEES = "invitees";

		protected override void WriteProperties(JsonWriter writer, Event value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.MeetingName != null)
			{
				writer.WritePropertyName(ATTR_TITLE);
				serializer.Serialize(writer, value.MeetingName);
			}

			if (value.MeetingStart != null)
			{
				writer.WritePropertyName(ATTR_START);
				serializer.Serialize(writer, value.MeetingStart);
			}

			if (value.MeetingEnd != null)
			{
				writer.WritePropertyName(ATTR_END);
				serializer.Serialize(writer, value.MeetingEnd);
			}

			if (value.OrganizerId != null)
			{
				writer.WritePropertyName(ATTR_CREATOR_ID);
				serializer.Serialize(writer, value.OrganizerId);
			}

			if (value.OrganizerEmail != null)
			{
				writer.WritePropertyName(ATTR_CREATOR_EMAIL);
				serializer.Serialize(writer, value.OrganizerEmail);
			}

			if (value.Id != null)
			{
				writer.WritePropertyName(ATTR_ID);
				serializer.Serialize(writer, value.Id);
			}

			if (value.Description != null)
			{
				writer.WritePropertyName(ATTR_DESCRIPTION);
				serializer.Serialize(writer, value.Description);
			}

			if (value.IsPrivate != null)
			{
				writer.WritePropertyName(ATTR_VISIBILITY);
				serializer.Serialize(writer, value.IsPrivate);
			}

			if (value.Invitees != null)
			{
				writer.WritePropertyName(ATTR_INVITEES);
				serializer.Serialize(writer, value.Invitees);
			}
		}

		/// <summary>
		/// Override to handle the current property value with the given name.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="reader"></param>
		/// <param name="instance"></param>
		/// <param name="serializer"></param>
		protected override void ReadProperty(string property, JsonReader reader, Event instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTR_TITLE:
					instance.MeetingName = reader.GetValueAsString();
					break;

				case ATTR_START:
					instance.MeetingStart = serializer.Deserialize<Event.DateInfo>(reader);
					break;

				case ATTR_END:
					instance.MeetingEnd = serializer.Deserialize<Event.DateInfo>(reader);
					break;

				case ATTR_CREATOR_ID:
					instance.OrganizerId = reader.GetValueAsString();
					break;

				case ATTR_CREATOR_EMAIL:
					instance.OrganizerEmail = reader.GetValueAsString();
					break;

				case ATTR_ID:
					instance.Id = reader.GetValueAsString();
					break;

				case ATTR_DESCRIPTION:
					instance.Description = reader.GetValueAsString();
					break;

				case ATTR_VISIBILITY:
					instance.IsPrivate = reader.GetValueAsString();
					break;

				case ATTR_INVITEES:
					instance.Invitees = (serializer.DeserializeArray<Event.EventInvitee>(reader) ?? new Event.EventInvitee[0]).ToArray();
					break;

				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}

	public sealed class DateInfoConverter : AbstractGenericJsonConverter<Event.DateInfo>
	{
		private const string ATTR_DATE_TIME = "date_time";
		private const string ATTR_TIME_ZONE = "time_zone";

		protected override void WriteProperties(JsonWriter writer, Event.DateInfo value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			writer.WritePropertyName(ATTR_DATE_TIME);
			serializer.Serialize(writer, value.DateTimeInfo);

			if (value.TimeZoneInfo != null)
			{
				writer.WritePropertyName(ATTR_TIME_ZONE);
				serializer.Serialize(writer, value.TimeZoneInfo);
			}
		}

		/// <summary>
		/// Override to handle the current property value with the given name.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="reader"></param>
		/// <param name="instance"></param>
		/// <param name="serializer"></param>
		protected override void ReadProperty(string property, JsonReader reader, Event.DateInfo instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTR_DATE_TIME:
					instance.DateTimeInfo = reader.GetValueAsDateTime().ToUniversalTime();
					break;

				case ATTR_TIME_ZONE:
					instance.TimeZoneInfo = reader.GetValueAsString();
					break;

				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}

	public sealed class DateInfoDateTimeConverter : IsoDateTimeConverter
	{
		public DateInfoDateTimeConverter()
		{
			base.DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
		}
	}

	public sealed class EventInviteeConverter : AbstractGenericJsonConverter<Event.EventInvitee>
	{
		private const string ATTR_ID = "id";
		private const string ATTR_EVENT_ID = "event_id";
		private const string ATTR_USER_ID = "user_id";
		private const string ATTR_EMAIL = "email";
		private const string ATTR_DISPLAY_NAME = "display_name";
		private const string ATTR_RESPONSE_STATUS = "response_status";
		private const string ATTR_IS_ORGANIZER = "is_organizer";
		private const string ATTR_IS_RESOURCE = "is_resource";
		private const string ATTR_UPDATED_AT = "updated_at";
		private const string ATTR_CREATED_AT = "created_at";

		protected override void WriteProperties(JsonWriter writer, Event.EventInvitee value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.Id != null)
			{
				writer.WritePropertyName(ATTR_ID);
				serializer.Serialize(writer, value.Id);
			}

			if (value.EventId != null)
			{
				writer.WritePropertyName(ATTR_EVENT_ID);
				serializer.Serialize(writer, value.EventId);
			}

			if (value.UserId != null)
			{
				writer.WritePropertyName(ATTR_USER_ID);
				serializer.Serialize(writer, value.UserId);
			}

			if (value.Email != null)
			{
				writer.WritePropertyName(ATTR_EMAIL);
				serializer.Serialize(writer, value.Email);
			}

			if (value.DisplayName != null)
			{
				writer.WritePropertyName(ATTR_DISPLAY_NAME);
				serializer.Serialize(writer, value.DisplayName);
			}

			if (value.ResponseStatus != null)
			{
				writer.WritePropertyName(ATTR_RESPONSE_STATUS);
				serializer.Serialize(writer, value.ResponseStatus);
			}

			if (value.IsOrganizer)
			{
				writer.WritePropertyName(ATTR_IS_ORGANIZER);
				serializer.Serialize(writer, value.IsOrganizer);
			}

			if (value.IsResource)
			{
				writer.WritePropertyName(ATTR_IS_RESOURCE);
				serializer.Serialize(writer, value.IsResource);
			}

			writer.WritePropertyName(ATTR_UPDATED_AT);
			serializer.Serialize(writer, value.UpdatedAt);

			writer.WritePropertyName(ATTR_CREATED_AT);
			serializer.Serialize(writer, value.CreatedAt);
		}

		protected override void ReadProperty(string property, JsonReader reader, Event.EventInvitee instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTR_ID:
					instance.Id = reader.GetValueAsString();
					break;
				case ATTR_EVENT_ID:
					instance.EventId = reader.GetValueAsString();
					break;
				case ATTR_USER_ID:
					instance.UserId = reader.GetValueAsString();
					break;
				case ATTR_EMAIL:
					instance.Email = reader.GetValueAsString();
					break;
				case ATTR_DISPLAY_NAME:
					instance.DisplayName = reader.GetValueAsString();
					break;
				case ATTR_RESPONSE_STATUS:
					instance.ResponseStatus = reader.GetValueAsString();
					break;
				case ATTR_IS_ORGANIZER:
					instance.IsOrganizer = reader.GetValueAsBool();
					break;
				case ATTR_IS_RESOURCE:
					instance.IsResource = reader.GetValueAsBool();
					break;
				case ATTR_UPDATED_AT:
					instance.UpdatedAt = reader.GetValueAsDateTime();
					break;
				case ATTR_CREATED_AT:
					instance.CreatedAt = reader.GetValueAsDateTime();
					break;

				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}