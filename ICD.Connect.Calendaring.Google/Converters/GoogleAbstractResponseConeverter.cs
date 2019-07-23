using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Converters
{
	public abstract class GoogleAbstractResponseConverter<T> : AbstractGenericJsonConverter<T> where T : AbstractGoogleResponse
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