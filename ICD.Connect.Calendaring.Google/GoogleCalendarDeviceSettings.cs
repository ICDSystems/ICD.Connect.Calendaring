using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Protocol.Network.Ports.Web;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Settings.Attributes;
using ICD.Connect.Settings.Attributes.SettingsProperties;

namespace ICD.Connect.Calendaring.Google
{
	[KrangSettings("GoogleCalendar", typeof(GoogleCalendarDevice))]
	public sealed class GoogleCalendarDeviceSettings : AbstractDeviceSettings, IUriSettings
	{
		private const string PORT_ELEMENT = "Port";
		private const string CALENDARPARSING_ELEMENT = "CalendarParsing";
		private const string CLIENT_EMAIL_ELEMENT = "ClientEmail";
		private const string CALENDAR_ID_ELEMENT = "CalendarId";
		private const string PRIVATE_KEY_ID_ELEMENT = "PrivateKey";
		private const string DEFAULT_CALENDAR_PARSING_PATH = "CalendarParsing.xml";

		private readonly UriProperties m_UriProperties;

		#region Properties

		/// <summary>
		/// The port id.
		/// </summary>
		[OriginatorIdSettingsProperty(typeof(IWebPort))]
		public int? Port { get; set; }
		public string CalendarParsingPath { get; set; }
		public string ClientEmail { get; set; }
		public string CalendarId { get; set; }
		public string PrivateKey { get; set; }


		#endregion

		#region URI

		/// <summary>
		/// Gets/sets the configurable URI username.
		/// </summary>
		public string UriUsername { get { return m_UriProperties.UriUsername; } set { m_UriProperties.UriUsername = value; } }

		/// <summary>
		/// Gets/sets the configurable URI password.
		/// </summary>
		public string UriPassword { get { return m_UriProperties.UriPassword; } set { m_UriProperties.UriPassword = value; } }

		/// <summary>
		/// Gets/sets the configurable URI host.
		/// </summary>
		public string UriHost { get { return m_UriProperties.UriHost; } set { m_UriProperties.UriHost = value; } }

		/// <summary>
		/// Gets/sets the configurable URI port.
		/// </summary>
		public ushort? UriPort { get { return m_UriProperties.UriPort; } set { m_UriProperties.UriPort = value; } }

		/// <summary>
		/// Gets/sets the configurable URI scheme.
		/// </summary>
		public string UriScheme { get { return m_UriProperties.UriScheme; } set { m_UriProperties.UriScheme = value; } }

		/// <summary>
		/// Gets/sets the configurable URI path.
		/// </summary>
		public string UriPath { get { return m_UriProperties.UriPath; } set { m_UriProperties.UriPath = value; } }

		/// <summary>
		/// Gets/sets the configurable URI query.
		/// </summary>
		public string UriQuery { get { return m_UriProperties.UriQuery; } set { m_UriProperties.UriQuery = value; } }

		/// <summary>
		/// Gets/sets the configurable URI fragment.
		/// </summary>
		public string UriFragment { get { return m_UriProperties.UriFragment; } set { m_UriProperties.UriFragment = value; } }

		/// <summary>
		/// Clears the configured values.
		/// </summary>
		void IUriProperties.ClearUriProperties()
		{
			m_UriProperties.ClearUriProperties();
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public GoogleCalendarDeviceSettings()
		{
			CalendarParsingPath = DEFAULT_CALENDAR_PARSING_PATH;

			m_UriProperties = new UriProperties();
			UpdateUriDefaults();
		}

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(PORT_ELEMENT, IcdXmlConvert.ToString(Port));
			writer.WriteElementString(CALENDARPARSING_ELEMENT, CalendarParsingPath);
			writer.WriteElementString(CLIENT_EMAIL_ELEMENT, ClientEmail);
			writer.WriteElementString(CALENDAR_ID_ELEMENT, CalendarId);
			writer.WriteElementString(PRIVATE_KEY_ID_ELEMENT, PrivateKey);

			m_UriProperties.WriteElements(writer);
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			Port = XmlUtils.TryReadChildElementContentAsInt(xml, PORT_ELEMENT);
			CalendarParsingPath = XmlUtils.TryReadChildElementContentAsString(xml, CALENDARPARSING_ELEMENT) ??
			                      DEFAULT_CALENDAR_PARSING_PATH;
			ClientEmail = XmlUtils.TryReadChildElementContentAsString(xml, CLIENT_EMAIL_ELEMENT);
			CalendarId = XmlUtils.TryReadChildElementContentAsString(xml, CALENDAR_ID_ELEMENT);
			PrivateKey = XmlUtils.TryReadChildElementContentAsString(xml, PRIVATE_KEY_ID_ELEMENT);

			m_UriProperties.ParseXml(xml);

			UpdateUriDefaults();
		}

		private void UpdateUriDefaults()
		{
			m_UriProperties.ApplyDefaultValuesFromAddress("https://www.googleapis.com/calendar/v3/");
		}
	}
}
