using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.Calendaring.CalendarParsers;
using ICD.Connect.Calendaring.Microsoft.Office365.Controls;
using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;
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
		private readonly WebProxyProperties m_WebProxyProperties;

		private readonly CalendarParserCollection m_CalendarParserCollection;

		private string m_CalendarParsingPath;
		private IWebPort m_Port;

		private string m_Token;
		private DateTime m_TokenExpireTime;

		#region Properties

		public string Tenant { get; set; }

		public string Client { get; set; }

		public string Secret { get; set; }

		public string UserEmail { get; set; }

		public CalendarParserCollection CalendarParserCollection { get { return m_CalendarParserCollection; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public Office365CalendarDevice()
		{
			m_UriProperties = new UriProperties();
			m_WebProxyProperties = new WebProxyProperties();

			m_CalendarParserCollection = new CalendarParserCollection();
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
			{
				port.ApplyDeviceConfiguration(m_UriProperties);
				port.ApplyDeviceConfiguration(m_WebProxyProperties);
			}
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

		/// <summary>
		/// Sets the Calendar Parsing from the settings.
		/// </summary>
		/// <param name="configPath"></param>
		private void SetCalendarParsers(string configPath)
		{
			m_CalendarParsingPath = configPath;

			try
			{
				m_CalendarParserCollection.LoadParsers(configPath);
			}
			catch (Exception e)
			{
				Logger.Log(eSeverity.Error, "Failed to load Calendar Parsers - {0}", e.Message);
			}
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

			m_UriProperties.Copy(settings);
			m_WebProxyProperties.Copy(settings);

			Tenant = settings.Tenant;
			Client = settings.Client;
			Secret = settings.Secret;
			UserEmail = settings.UserEmail;

			SetCalendarParsers(settings.CalendarParsingPath);

			IWebPort port = null;

			if (settings.Port != null)
			{
				try
				{
					port = factory.GetPortById((int)settings.Port) as IWebPort;
				}
				catch (KeyNotFoundException)
				{
					Logger.Log(eSeverity.Error, "No web port with id {0}", settings.Port);
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

			m_CalendarParserCollection.ClearMatchers();

			Tenant = null;
			Client = null;
			Secret = null;
			UserEmail = null;

			SetPort(null);

			m_UriProperties.ClearUriProperties();
			m_WebProxyProperties.ClearProxyProperties();
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(Office365CalendarDeviceSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.CalendarParsingPath = m_CalendarParsingPath;

			settings.Tenant = Tenant;
			settings.Client = Client;
			settings.Secret = Secret;
			settings.UserEmail = UserEmail;

			settings.Port = m_Port == null ? (int?)null : m_Port.Id;

			settings.Copy(m_UriProperties);
			settings.Copy(m_WebProxyProperties);
		}

		/// <summary>
		/// Override to add controls to the device.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		/// <param name="addControl"></param>
		protected override void AddControls(Office365CalendarDeviceSettings settings, IDeviceFactory factory, Action<IDeviceControl> addControl)
		{
			base.AddControls(settings, factory, addControl);

			addControl(new Office365CalendarControl(this, Controls.Count));
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
		}

		private string RenewToken()
		{
			// Set port accept type
			m_Port.Accept = "*/*";

			// Build request header
			Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>();
			
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
			WebPortResponse output = m_Port.Post(url, headers, bodyData);
			if (!output.Success)
			{
				Logger.Log(eSeverity.Error, "Failed to get token - {0}", output.DataAsString);
				m_Token = null;
				return null;
			}

			// Get the token string value out of the JSON
			TokenResponse response = JsonConvert.DeserializeObject<TokenResponse>(output.DataAsString);

			m_Token = response.AccessToken;
			m_TokenExpireTime = IcdEnvironment.GetUtcTime() + new TimeSpan(0, 0, response.ExpiresInSeconds);

			return m_Token;
		}

		public IEnumerable<CalendarEvent> GetEvents()
		{
			if (m_Token == null || IcdEnvironment.GetUtcTime() >= m_TokenExpireTime)
				RenewToken();

			Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>
			{
				{"Authorization", new List<string>{"Bearer " + m_Token}},
				{"Prefer", new List<string>{"outlook.timezone=UTC"}}
			};

			string url = string.Format("https://graph.microsoft.com/v1.0/users/{0}/calendarview?startDateTime={1}&endDateTime={2}",
			                           UserEmail,
			                           IcdEnvironment.GetLocalTime().StartOfDay().ToUniversalTime().ToString("o"),
			                           IcdEnvironment.GetLocalTime().EndOfDay().ToUniversalTime().ToString("o"));

			
			WebPortResponse getResponse = m_Port.Get(url, headers);

			CalendarViewResponse response = null;
			if (!string.IsNullOrEmpty(getResponse.DataAsString))
				response = JsonConvert.DeserializeObject<CalendarViewResponse>(getResponse.DataAsString);

			if (!getResponse.Success)
			{
				if (response != null && response.Error != null)
					throw new InvalidOperationException(response.Error.Message);
				throw new InvalidOperationException("Request failed");
			}

			return response == null
				? Enumerable.Empty<CalendarEvent>()
				: response.Value;
		}

		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		#endregion
	}
}
