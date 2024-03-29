﻿#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class ErrorConverter : AbstractGenericJsonConverter<Error>
	{
		private const string ATTRIBUTE_CODE = "code";
		private const string ATTRIBUTE_MESSAGE = "message";
		private const string ATTRIBUTE_INNER_ERROR = "innerError";

		protected override void ReadProperty(string property, JsonReader reader, Error instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_CODE:
					instance.Code = reader.GetValueAsString();
					break;
				case ATTRIBUTE_MESSAGE:
					instance.Message = reader.GetValueAsString();
					break;
				case ATTRIBUTE_INNER_ERROR:
					instance.InnerError = serializer.Deserialize<InnerError>(reader);
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}