using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Robin.Components.Users;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Components.Converters
{
	public sealed class UserConverter : AbstractGenericJsonConverter<User>
	{
		private const string ATTR_ID = "id";
		private const string ATTR_NAME = "name";
		private const string ATTR_TIME_ZONE = "time_zone";
		private const string ATTR_PRIMARY_EMAIL = "primary_email";

		/// <summary>
		/// Override to handle the current property value with the given name.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="reader"></param>
		/// <param name="instance"></param>
		/// <param name="serializer"></param>
		protected override void ReadProperty(string property, JsonReader reader, User instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTR_ID:
					instance.Id = reader.GetValueAsString();
					break;

				case ATTR_NAME:
					instance.UserName = reader.GetValueAsString();
					break;

				case ATTR_TIME_ZONE:
					instance.TimeZone = reader.GetValueAsString();
					break;

				case ATTR_PRIMARY_EMAIL:
					instance.Email = serializer.Deserialize<User.EmailInfo>(reader);
					break;

				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}

	public sealed class EmailInfoConverter : AbstractGenericJsonConverter<User.EmailInfo>
	{
		private const string ATTR_EMAIL = "email";
		private const string ATTR_IS_VERIFIED = "is_verified";

		/// <summary>
		/// Override to handle the current property value with the given name.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="reader"></param>
		/// <param name="instance"></param>
		/// <param name="serializer"></param>
		protected override void ReadProperty(string property, JsonReader reader, User.EmailInfo instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTR_EMAIL:
					instance.Email = reader.GetValueAsString();
					break;

				case ATTR_IS_VERIFIED:
					instance.Verified = reader.GetValueAsBool();
					break;

				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}
