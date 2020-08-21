using ICD.Common.Utils.Xml;
using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Devices.Points;

namespace ICD.Connect.Calendaring.CalendarPoints
{
	public abstract class AbstractCalendarPointSettings : AbstractPointSettings, ICalendarPointSettings
	{
		private const string FEATURES_ELEMENT = "Features";

		public eCalendarFeatures Features { get; set; }

		/// <summary>
		/// Write property elements to xml
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(FEATURES_ELEMENT, IcdXmlConvert.ToString(Features));
		}

		/// <summary>
		/// Instantiate volume point settings from an xml element
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			Features = XmlUtils.TryReadChildElementContentAsEnum<eCalendarFeatures>(xml, FEATURES_ELEMENT, true) ??
			           eCalendarFeatures.None;
		}
	}
}