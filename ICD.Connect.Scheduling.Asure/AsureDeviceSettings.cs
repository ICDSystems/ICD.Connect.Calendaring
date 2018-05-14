using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Protocol.Network.WebPorts;
using ICD.Connect.Settings.Attributes;
using ICD.Connect.Settings.Attributes.SettingsProperties;

namespace ICD.Connect.Scheduling.Asure
{
	[KrangSettings("AsureService", typeof(AsureDevice))]
	public sealed class AsureDeviceSettings : AbstractDeviceSettings, IUriProperties
	{
		private const string RESOURCE_ID_ELEMENT = "ResourceId";
		private const string UPDATE_INTERVAL_ELEMENT = "UpdateInterval";
		private const string PORT_ELEMENT = "Port";

		private readonly UriProperties m_UriProperties;

		#region Properties

		[OriginatorIdSettingsProperty(typeof(IWebPort))]
		public int? Port { get; set; }

		public int ResourceId { get; set; }

		public long? UpdateInterval { get; set; }

		#endregion

		#region URI

		/// <summary>
		/// Gets/sets the configurable username.
		/// </summary>
		public string Username { get { return m_UriProperties.Username; } set { m_UriProperties.Username = value; } }

		/// <summary>
		/// Gets/sets the configurable password.
		/// </summary>
		public string Password { get { return m_UriProperties.Password; } set { m_UriProperties.Password = value; } }

		/// <summary>
		/// Gets/sets the configurable network address.
		/// </summary>
		public string NetworkAddress { get { return m_UriProperties.NetworkAddress; } set { m_UriProperties.NetworkAddress = value; } }

		/// <summary>
		/// Gets/sets the configurable network port.
		/// </summary>
		public ushort NetworkPort { get { return m_UriProperties.NetworkPort; } set { m_UriProperties.NetworkPort = value; } }

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

		#endregion

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

			m_UriProperties.WriteElements(writer);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public AsureDeviceSettings()
		{
			//https://rsapi.resourcescheduler.net/ResourceScheduler.WebService/ResourceSchedulerService.asmx

			m_UriProperties = new UriProperties
			{
				UriScheme = "https",
				UriPath = "/ResourceScheduler.WebService/ResourceSchedulerService.asmx",
			};
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

			m_UriProperties.ParseXml(xml);
		}
	}
}
