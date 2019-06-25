using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Calendaring.Microsoft.Exchange
{
	
	[KrangSettings("ExachangeCalendar", typeof(ExchangeCalendarDevice))]
	public sealed class ExchangeCalendarDeviceSettings : AbstractDeviceSettings
	{
		private const string USERNAME_ELEMENT = "Username";
		private const string PASSWORD_ELEMENT = "Password";
		private const string URL_ELEMENT = "Url";

		#region Properties

		public string Username { get; set; }

		public string Password { get; set; }

		public string Url { get; set; }

		#endregion

		///<summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name ="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(USERNAME_ELEMENT, IcdXmlConvert.ToString(Username));
			writer.WriteElementString(PASSWORD_ELEMENT, IcdXmlConvert.ToString(Password));
			writer.WriteElementString(URL_ELEMENT, IcdXmlConvert.ToString(Url));
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);
			
			Username = XmlUtils.TryReadChildElementContentAsString(xml, USERNAME_ELEMENT);
			Password = XmlUtils.TryReadChildElementContentAsString(xml, PASSWORD_ELEMENT);
			Url = XmlUtils.TryReadChildElementContentAsString(xml, URL_ELEMENT);
		}
	}
}
