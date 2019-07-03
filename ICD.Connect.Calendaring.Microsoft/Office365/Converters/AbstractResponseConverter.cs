using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public abstract class AbstractResponseConverter<T> : AbstractGenericJsonConverter<T>
		where T : AbstractResponse
	{
		private const string ATTRIBUTE_ERROR = "error";

		protected override void ReadProperty(string property, JsonReader reader, T instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_ERROR:
					instance.Error = serializer.Deserialize<Error>(reader);
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}