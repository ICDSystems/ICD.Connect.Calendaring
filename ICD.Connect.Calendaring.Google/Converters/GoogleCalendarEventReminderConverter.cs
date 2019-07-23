using System.Runtime.CompilerServices;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public sealed class GoogleCalendarEventReminderConverter : AbstractGenericJsonConverter<GoogleCalendarEventReminder>
	{
		private const string ATTRIBUTE_USEDEFAUlT = "useDefault";
		protected override void ReadProperty(string property, JsonReader reader, GoogleCalendarEventReminder instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_USEDEFAUlT:
					instance.UseDefault = reader.GetValueAsBool();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}

		}


	}
}
