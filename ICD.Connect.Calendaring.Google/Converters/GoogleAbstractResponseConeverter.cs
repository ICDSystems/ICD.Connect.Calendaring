#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Google.Responses;

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