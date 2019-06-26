using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using ICD.Connect.Devices;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Protocol.Extensions;
using ICD.Connect.Protocol.Network.Ports.Web;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Protocol.Network.Utils;
using ICD.Connect.Settings;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Microsoft.Office365
{
	public sealed class Office365CalendarDevice : AbstractDevice<Office365CalendarDeviceSettings>
	{
		private readonly UriProperties m_UriProperties;
		private readonly SafeCriticalSection m_ThisSection;

		private IWebPort m_Port;
		private string m_Token;

		#region Properties

		public string Tenant { get; set; }

		public string Client { get; set; }

		public string Secret { get; set; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public Office365CalendarDevice()
		{
			m_UriProperties = new UriProperties();
			m_ThisSection = new SafeCriticalSection();

			Controls.Add(new Office365CalendarDeviceCalendarControl(this, Controls.Count));
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			base.DisposeFinal(disposing);

			SetPort(null);
		}

		#region Methods

		/// <summary>
		/// Sets the port for communication with the service.
		/// </summary>
		/// <param name="port"></param>
		[PublicAPI]
		public void SetPort(IWebPort port)
		{
			if (port == m_Port)
				return;

			ConfigurePort(port);

			Unsubscribe(m_Port);

			if (port != null)
				port.Accept = "application/json";

			m_Port = port;
			Subscribe(m_Port);

			UpdateCachedOnlineStatus();
		}

		/// <summary>
		/// Configures the given port for communication with the device.
		/// </summary>
		/// <param name="port"></param>
		private void ConfigurePort(IWebPort port)
		{
			// URI
			if (port != null)
				port.ApplyDeviceConfiguration(m_UriProperties);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the current online status of the device.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
			return m_Port != null && m_Port.IsOnline;
		}

		/// <summary>
		/// Subscribe to the port events.
		/// </summary>
		/// <param name="port"></param>
		private void Subscribe(IWebPort port)
		{
			if (port == null)
				return;

			port.OnIsOnlineStateChanged += PortOnIsOnlineStateChanged;
		}

		/// <summary>
		/// Unsubscribe from the port events.
		/// </summary>
		/// <param name="port"></param>
		private void Unsubscribe(IWebPort port)
		{
			if (port == null)
				return;

			port.OnIsOnlineStateChanged -= PortOnIsOnlineStateChanged;
		}

		/// <summary>
		/// Called when the port online state changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void PortOnIsOnlineStateChanged(object sender, DeviceBaseOnlineStateApiEventArgs eventArgs)
		{
			UpdateCachedOnlineStatus();
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(Office365CalendarDeviceSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			Tenant = settings.Tenant;
			Client = settings.Client;
			Secret = settings.Secret;

			m_UriProperties.Copy(settings);

			IWebPort port = null;

			if (settings.Port != null)
			{
				try
				{
					port = factory.GetPortById((int)settings.Port) as IWebPort;
				}
				catch (KeyNotFoundException)
				{
					Log(eSeverity.Error, "No web port with id {0}", settings.Port);
				}
			}

			SetPort(port);
		}

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			Tenant = null;
			Client = null;
			Secret = null;

			SetPort(null);

			m_UriProperties.Clear();
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(Office365CalendarDeviceSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.Tenant = Tenant;
			settings.Client = Client;
			settings.Secret = Secret;

			settings.Port = m_Port == null ? (int?)null : m_Port.Id;

			settings.Copy(m_UriProperties);
		}

		#endregion

		#region Console

		/// <summary>
		/// Gets the help information for the node.
		/// </summary>
		public override string ConsoleHelp { get { return "The Exchange service device"; } }

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			yield return new ConsoleCommand("RenewToken", "", () => RenewToken());
			//yield return new ConsoleCommand("GetEvents", "", () => GetEvents());

		}

		private string RenewToken()
		{
			// Set port accept type
			m_Port.Accept = "*/*";

			// Build request header
			Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>
			{
				{"Content-Type", new List<string> {"application/x-www-form-urlencoded"}}
			};
			
			// Build request body
			Dictionary<string, string> body = new Dictionary<string, string>
			{
				{"client_id", Client},
				{"grant_type", "client_credentials"},
				{"client_secret", Secret},
				{"scope", "https://graph.microsoft.com/.default"}
			};

			byte[] bodyData = HttpUtils.GetFormUrlEncodedContentBytes(body);

			string scope = Uri.EscapeDataString("https://graph.microsoft.com/.default");
			string url = string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0/token?scope={1}", Tenant, scope);

			// Dispatch!
			string result;
			if (!m_Port.Post(url, headers, bodyData, out result))
			{
				Log(eSeverity.Error, "Failed to get token - {0}", result);
				m_Token = null;
				return null;
			}

			// Get the token string value out of the JSON
			TokenResponse response = JsonConvert.DeserializeObject<TokenResponse>(result);
			m_Token = response.AccessToken;

			return m_Token;
		}

		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		#endregion
	}
}
