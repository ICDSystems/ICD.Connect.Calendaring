using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365.Converters
{
	public sealed class CalendarViewResponseConverter : AbstractGenericJsonConverter<CalendarViewResponse>
	{
		private const string ATTRIBUTE_DATA_CONTEXT = "@odata.context";
		private const string ATTRIBUTE_VALUE = "value";

		/// <summary>
		/// Override to handle the current property value with the given name.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="reader"></param>
		/// <param name="instance"></param>
		/// <param name="serializer"></param>
		protected override void ReadProperty(string property, JsonReader reader, CalendarViewResponse instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_DATA_CONTEXT:
					instance.DataContext = reader.GetValueAsString();
					break;

				case ATTRIBUTE_VALUE:
					instance.Value = serializer.DeserializeArray<CalendarEvent>(reader).ToArray();
					break;
				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}