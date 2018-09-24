using System;
using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Protocol.Network.WebPorts;
using ICD.Connect.Settings.Attributes;
using ICD.Connect.Settings.Attributes.SettingsProperties;

namespace ICD.Connect.Calendaring.Asure
{
	[KrangSettings(FACTORY_NAME)]
	public sealed class AsureDeviceSettings : AbstractDeviceSettings
	{
		private const string FACTORY_NAME = "AsureService";

		private const string RESOURCE_ID_ELEMENT = "ResourceId";
		private const string UPDATE_INTERVAL_ELEMENT = "UpdateInterval";
		private const string PORT_ELEMENT = "Port";
		private const string USERNAME_ELEMENT = "Username";
		private const string PASSWORD_ELEMENT = "Password";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(AsureDevice); } }

		[OriginatorIdSettingsProperty(typeof(IWebPort))]
		public int? Port { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }

		public int ResourceId { get; set; }
		public long? UpdateInterval { get; set; }

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
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			int? port = XmlUtils.TryReadChildElementContentAsInt(xml, PORT_ELEMENT);
			int resourceId = XmlUtils.TryReadChildElementContentAsInt(xml, RESOURCE_ID_ELEMENT) ?? 0;
			long? updateInterval = XmlUtils.TryReadChildElementContentAsLong(xml, UPDATE_INTERVAL_ELEMENT);
			string username = XmlUtils.TryReadChildElementContentAsString(xml, USERNAME_ELEMENT);
			string password = XmlUtils.TryReadChildElementContentAsString(xml, PASSWORD_ELEMENT);

			Port = port;
			ResourceId = resourceId;
			UpdateInterval = updateInterval;
			Username = username;
			Password = password;
		}
	}
}
