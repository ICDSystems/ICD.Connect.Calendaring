using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public sealed class GoogleCalendarEventAttendeeConverter : AbstractGenericJsonConverter<GoogleCalendarEventAttendee>
	{
		private const string ATTRIBUTE_EMAIL = "email";
		private const string ATTRIBUTE_SELF = "self";
		private const string ATTRIBUTE_OPTIONAL = "optional";
		private const string ATTRIBUTE_DISPLAY_NAME = "displayName";
		private const string ATTRIBUTE_RESOURCE = "resource";
		private const string ATTRIBUTE_ORGANIZER = "organizer";
		private const string ATTRIBUTE_RESPONSE_STATUS = "responseStatus";

		protected override void WriteProperties(JsonWriter writer, GoogleCalendarEventAttendee value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.Email != null)
			{
				writer.WritePropertyName(ATTRIBUTE_EMAIL);
				serializer.Serialize(writer, value.Email);
			}

			if (value.Self)
			{
				writer.WritePropertyName(ATTRIBUTE_SELF);
				serializer.Serialize(writer, value.Self);
			}

			if (value.Optional)
			{
				writer.WritePropertyName(ATTRIBUTE_OPTIONAL);
				serializer.Serialize(writer, value.Optional);
			}

			if (value.DisplayName != null)
			{
				writer.WritePropertyName(ATTRIBUTE_DISPLAY_NAME);
				serializer.Serialize(writer, value.DisplayName);
			}

			if (value.Resource != null)
			{
				writer.WritePropertyName(ATTRIBUTE_RESOURCE);
				serializer.Serialize(writer, value.Resource);
			}

			if (value.Organizer)
			{
				writer.WritePropertyName(ATTRIBUTE_RESOURCE);
				serializer.Serialize(writer, value.Organizer);
			}

			if (value.ResponseStatus != null)
			{
				writer.WritePropertyName(ATTRIBUTE_RESPONSE_STATUS);
				serializer.Serialize(writer, value.ResponseStatus);
			}
		}

		protected override void ReadProperty(string property, JsonReader reader, GoogleCalendarEventAttendee instance,JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_EMAIL:
					instance.Email = reader.GetValueAsString();
					break;
				case ATTRIBUTE_SELF:
					instance.Self = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_OPTIONAL:
					instance.Optional = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_DISPLAY_NAME:
					instance.DisplayName = reader.GetValueAsString();
					break;
				case ATTRIBUTE_RESOURCE:
					instance.Resource = reader.GetValueAsString();
					break;
				case ATTRIBUTE_ORGANIZER:
					instance.Organizer = reader.GetValueAsBool();
					break;
				case ATTRIBUTE_RESPONSE_STATUS:
					instance.ResponseStatus = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}

	}
}