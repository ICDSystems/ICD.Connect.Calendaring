using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Protocol.Network.WebPorts;
using ICD.Connect.Settings.Attributes;
using ICD.Connect.Settings.Attributes.SettingsProperties;

namespace ICD.Connect.Calendaring.Robin
{
	[KrangSettings("RobinServiceDevice", typeof(RobinServiceDevice))]
	public sealed class RobinServiceDeviceSettings : AbstractDeviceSettings
    {
		private const string PORT_ELEMENT = "Port";
        private const string TOKEN_ELEMENT = "Token";
        private const string RESOURCEID_ELEMENT = "ResourceId";

        /// <summary>
        /// The port id.
        /// </summary>
        [OriginatorIdSettingsProperty(typeof(IWebPort))]
		public int? Port { get; set; }

        public string Token { get; set; }

        public string ResourceId { get; set; }

        /// <summary>
        /// Writes property elements to xml.
        /// </summary>
        /// <param name="writer"></param>
        protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(PORT_ELEMENT, IcdXmlConvert.ToString(Port));
		    writer.WriteElementString(TOKEN_ELEMENT, Token);
		    writer.WriteElementString(RESOURCEID_ELEMENT, ResourceId);
        }

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			Port = XmlUtils.TryReadChildElementContentAsInt(xml, PORT_ELEMENT);
		    Token = XmlUtils.TryReadChildElementContentAsString(xml, TOKEN_ELEMENT);
		    ResourceId = XmlUtils.TryReadChildElementContentAsString(xml, RESOURCEID_ELEMENT);
        }
	}
}