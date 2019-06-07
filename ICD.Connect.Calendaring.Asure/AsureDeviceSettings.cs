using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Protocol.Network.WebPorts;
using ICD.Connect.Settings.Attributes;
using ICD.Connect.Settings.Attributes.SettingsProperties;

namespace ICD.Connect.Calendaring.Asure
{
	[KrangSettings("AsureService", typeof(AsureDevice))]
	public sealed class AsureDeviceSettings : AbstractDeviceSettings
	{
		private const string RESOURCE_ID_ELEMENT = "ResourceId";
		private const string UPDATE_INTERVAL_ELEMENT = "UpdateInterval";
		private const string PORT_ELEMENT = "Port";
		private const string USERNAME_ELEMENT = "Username";
		private const string PASSWORD_ELEMENT = "Password";
		private const string CALENDARPARSING_ELEMENT = "CalendarParsing";

		[OriginatorIdSettingsProperty(typeof(IWebPort))]
		public int? Port { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }
		public int ResourceId { get; set; }
		public long? UpdateInterval { get; set; }
		public string CalendarParsingPath { get; set; }

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(RESOURCE_ID_ELEMENT, IcdXmlConvert.ToString(ResourceId));
			writer.WriteElementString(UPDATE_INTERVAL_ELEMENT, IcdXmlConvert.ToString(UpdateInterval));
			writer.WriteElementString(PORT_ELEMENT, IcdXmlConvert.ToString(Port));
			writer.WriteElementString(USERNAME_ELEMENT, Username);
			writer.WriteElementString(PASSWORD_ELEMENT, Password);
			writer.WriteElementString(CALENDARPARSING_ELEMENT, CalendarParsingPath);
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			Port = XmlUtils.TryReadChildElementContentAsInt(xml, PORT_ELEMENT);
			ResourceId = XmlUtils.TryReadChildElementContentAsInt(xml, RESOURCE_ID_ELEMENT) ?? 0;
			UpdateInterval = XmlUtils.TryReadChildElementContentAsLong(xml, UPDATE_INTERVAL_ELEMENT);
			Username = XmlUtils.TryReadChildElementContentAsString(xml, USERNAME_ELEMENT);
			Password = XmlUtils.TryReadChildElementContentAsString(xml, PASSWORD_ELEMENT);
			CalendarParsingPath = XmlUtils.TryReadChildElementContentAsString(xml, CALENDARPARSING_ELEMENT);
		}
	}
}
