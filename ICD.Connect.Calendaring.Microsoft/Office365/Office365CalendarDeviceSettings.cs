﻿using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Protocol.Network.Ports.Web;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Settings.Attributes;
using ICD.Connect.Settings.Attributes.SettingsProperties;

namespace ICD.Connect.Calendaring.Microsoft.Office365
{
	[KrangSettings("Office365Calendar", typeof(Office365CalendarDevice))]
	public sealed class Office365CalendarDeviceSettings : AbstractDeviceSettings, IUriSettings, IWebProxySettings
	{
		private const string PORT_ELEMENT = "Port";
		private const string TENANT_ELEMENT = "Tenant";
		private const string CLIENT_ELEMENT = "Client";
		private const string SECRET_ELEMENT = "Secret";
		private const string USER_EMAIL_ELEMENT = "UserEmail";
		private const string CALENDARPARSING_ELEMENT = "CalendarParsing";

		private const string DEFAULT_CALENDAR_PARSING_PATH = "CalendarParsing.xml";

		private readonly UriProperties m_UriProperties;
		private readonly WebProxyProperties m_WebProxyProperties;

		#region Properties

		/// <summary>
		/// The port id.
		/// </summary>
		[OriginatorIdSettingsProperty(typeof(IWebPort))]
		public int? Port { get; set; }

		public string Tenant { get; set; }

		public string Client { get; set; }
		
		public string Secret { get; set; }

		public string UserEmail { get; set; }

		public string CalendarParsingPath { get; set; }

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

		#region Proxy

		/// <summary>
		/// Gets/sets the configurable proxy username.
		/// </summary>
		public string ProxyUsername { get { return m_WebProxyProperties.ProxyUsername; } set { m_WebProxyProperties.ProxyUsername = value; } }

		/// <summary>
		/// Gets/sets the configurable proxy password.
		/// </summary>
		public string ProxyPassword { get { return m_WebProxyProperties.ProxyPassword; } set { m_WebProxyProperties.ProxyPassword = value; } }

		/// <summary>
		/// Gets/sets the configurable proxy host.
		/// </summary>
		public string ProxyHost { get { return m_WebProxyProperties.ProxyHost; } set { m_WebProxyProperties.ProxyHost = value; } }

		/// <summary>
		/// Gets/sets the configurable proxy port.
		/// </summary>
		public ushort? ProxyPort { get { return m_WebProxyProperties.ProxyPort; } set { m_WebProxyProperties.ProxyPort = value; } }

		/// <summary>
		/// Gets/sets the configurable proxy scheme.
		/// </summary>
		public string ProxyScheme { get { return m_WebProxyProperties.ProxyScheme; } set { m_WebProxyProperties.ProxyScheme = value; } }

		/// <summary>
		/// Gets/sets the configurable proxy authentication method.
		/// </summary>
		public eProxyAuthenticationMethod? ProxyAuthenticationMethod
		{
			get { return m_WebProxyProperties.ProxyAuthenticationMethod; }
			set { m_WebProxyProperties.ProxyAuthenticationMethod = value; }
		}

		/// <summary>
		/// Clears the configured values.
		/// </summary>
		public void ClearProxyProperties()
		{
			m_WebProxyProperties.ClearProxyProperties();
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public Office365CalendarDeviceSettings()
		{
			CalendarParsingPath = DEFAULT_CALENDAR_PARSING_PATH;

			m_UriProperties = new UriProperties();
			m_WebProxyProperties = new WebProxyProperties();

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
			writer.WriteElementString(TENANT_ELEMENT, Tenant);
			writer.WriteElementString(CLIENT_ELEMENT, Client);
			writer.WriteElementString(SECRET_ELEMENT, Secret);
			writer.WriteElementString(USER_EMAIL_ELEMENT, UserEmail);
			writer.WriteElementString(CALENDARPARSING_ELEMENT, CalendarParsingPath);

			m_UriProperties.WriteElements(writer);
			m_WebProxyProperties.WriteElements(writer);
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			Port = XmlUtils.TryReadChildElementContentAsInt(xml, PORT_ELEMENT);
			Tenant = XmlUtils.TryReadChildElementContentAsString(xml, TENANT_ELEMENT);
			Client = XmlUtils.TryReadChildElementContentAsString(xml, CLIENT_ELEMENT);
			Secret = XmlUtils.TryReadChildElementContentAsString(xml, SECRET_ELEMENT);
			UserEmail = XmlUtils.TryReadChildElementContentAsString(xml, USER_EMAIL_ELEMENT);
			CalendarParsingPath = XmlUtils.TryReadChildElementContentAsString(xml, CALENDARPARSING_ELEMENT) ??
			                      DEFAULT_CALENDAR_PARSING_PATH;

			m_UriProperties.ParseXml(xml);
			m_WebProxyProperties.ParseXml(xml);

			UpdateUriDefaults();
		}

		private void UpdateUriDefaults()
		{
			m_UriProperties.ApplyDefaultValuesFromAddress("https://graph.microsoft.com/v2.0/");
		}
	}
}
