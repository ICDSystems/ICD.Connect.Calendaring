using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Calendaring.Microsoft.Exchange
{
	[KrangSettings("ExchangeCalendar", typeof(ExchangeCalendarDevice))]
	public sealed class ExchangeCalendarDeviceSettings : AbstractDeviceSettings
	{
		private const string CALENDARPARSING_ELEMENT = "CalendarParsing";
		private const string USERNAME_ELEMENT = "Username";
		private const string PASSWORD_ELEMENT = "Password";

		private const string DEFAULT_CALENDAR_PARSING_PATH = "CalendarParsing.xml";

		#region Properties

		public string CalendarParsingPath { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public ExchangeCalendarDeviceSettings()
		{
			CalendarParsingPath = DEFAULT_CALENDAR_PARSING_PATH;
		}

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(CALENDARPARSING_ELEMENT, CalendarParsingPath);
			writer.WriteElementString(USERNAME_ELEMENT, Username);
			writer.WriteElementString(PASSWORD_ELEMENT, Password);
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			CalendarParsingPath = XmlUtils.TryReadChildElementContentAsString(xml, CALENDARPARSING_ELEMENT) ??
			                      DEFAULT_CALENDAR_PARSING_PATH;
			Username = XmlUtils.TryReadChildElementContentAsString(xml, USERNAME_ELEMENT);
			Password = XmlUtils.TryReadChildElementContentAsString(xml, PASSWORD_ELEMENT);
		}
	}
}