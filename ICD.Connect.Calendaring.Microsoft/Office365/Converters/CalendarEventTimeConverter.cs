﻿#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using System;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarEventTimeConverter : AbstractGenericJsonConverter<CalendarEventTime>
	{
		private const string ATTRIBUTE_DATE_TIME = "dateTime";
		private const string ATTRIBUTE_TIME_ZONE = "timeZone";

		protected override void WriteProperties(JsonWriter writer, CalendarEventTime value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			writer.WritePropertyName(ATTRIBUTE_DATE_TIME);
			serializer.Serialize(writer, value.DateTime);

			if (value.TimeZone != null)
			{
				writer.WritePropertyName(ATTRIBUTE_TIME_ZONE);
				serializer.Serialize(writer, value.TimeZone);
			}
		}

		protected override void ReadProperty(string property, JsonReader reader, CalendarEventTime instance,JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_DATE_TIME:
					// Hack - Microsoft doesn't give us the Z, but we're always asking for times in UTC
					DateTime badLocal = reader.GetValueAsDateTime();
					string data = badLocal.ToString("o");
					instance.DateTime = DateTime.Parse(data + 'Z');
					break;
				case ATTRIBUTE_TIME_ZONE:
					instance.TimeZone = reader.GetValueAsString();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}