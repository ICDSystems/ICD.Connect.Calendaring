using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Robin.Components.Events;
using Newtonsoft.Json;

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
}