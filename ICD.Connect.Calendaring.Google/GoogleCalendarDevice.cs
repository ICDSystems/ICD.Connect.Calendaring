﻿#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
using Formatting = RealNewtonsoft.Newtonsoft.Json.Formatting;
#else
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#if SIMPLSHARP
using Crestron.SimplSharp.CrestronXml;
#else
using System.Xml;
#endif
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.Calendaring.CalendarParsers;
using ICD.Connect.Calendaring.Google.Controls;
using ICD.Connect.Calendaring.Google.Responses;
using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Protocol.Extensions;
using ICD.Connect.Protocol.Network.Ports.Web;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Protocol.Network.Utils;
using ICD.Connect.Settings;

namespace ICD.Connect.Calendaring.Google
{
	public sealed class GoogleCalendarDevice : AbstractDevice<GoogleCalendarDeviceSettings>
	{
		private readonly UriProperties m_UriProperties;
		private readonly WebProxyProperties m_WebProxyProperties;

		private readonly CalendarParserCollection m_CalendarParserCollection;

		private string m_CalendarParsingPath;
		private IWebPort m_Port;
		private string m_Token;
		private DateTime m_TokenExpireTime;

		#region Properties

		public string ClientEmail { get; set; }
		public string CalendarId { get; set; }
		public string PrivateKey { get; set; }
		public CalendarParserCollection CalendarParserCollection { get { return m_CalendarParserCollection; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public GoogleCalendarDevice()
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

		public IEnumerable<GoogleCalendarEvent> GetEvents()
		{
			if (m_Token == null || IcdEnvironment.GetUtcTime() >= m_TokenExpireTime)
				RenewToken();

			// We want the events to be in UTC at this level.
			const string responseTimeZone = "UTC";

			// Format the URL with query parameters requesting the current days events.
			string url = string.Format("https://www.googleapis.com/calendar/v3/calendars/{0}/events?access_token={1}&singleEvents=true&timeMax={2}&timeMin={3}&timeZone={4}",
			                           CalendarId, m_Token, Rfc3339EndOfDayTimeStamp(), Rfc3339StartOfDayTimeStamp(), responseTimeZone);

			WebPortResponse getResponse = m_Port.Get(url, null);
			if (!getResponse.GotResponse)
				throw new InvalidOperationException("Request failed");

			GoogleCalendarViewResponse response = null;
			if (!string.IsNullOrEmpty(getResponse.DataAsString))
				response = JsonConvert.DeserializeObject<GoogleCalendarViewResponse>(getResponse.DataAsString);

			if (!getResponse.IsSuccessCode)
			{
				if (response != null && response.Error != null)
					throw new InvalidOperationException(response.Error.Message);
				throw new InvalidOperationException("Request failed");
			}

			return response == null
				       ? Enumerable.Empty<GoogleCalendarEvent>()
				       : response.Items;

		}

		public void CreateEvent(GoogleCalendarEvent newEvent)
		{
			if (m_Token == null || IcdEnvironment.GetUtcTime() >= m_TokenExpireTime)
				RenewToken();

			string url = string.Format("https://www.googleapis.com/calendar/v3/calendars/{0}/events?access_token={1}",
			                           CalendarId, m_Token);

			string eventData = JsonConvert.SerializeObject(newEvent, Formatting.None);

			WebPortResponse postResponse =
				m_Port.Post(url, Encoding.UTF8.GetBytes(eventData));
			if (!postResponse.GotResponse)
				throw new InvalidOperationException("Request failed");

			GoogleCalendarViewResponse response = null;
			if (!string.IsNullOrEmpty(postResponse.DataAsString))
				response = JsonConvert.DeserializeObject<GoogleCalendarViewResponse>(postResponse.DataAsString);
			if (!postResponse.IsSuccessCode)
			{
				if (response != null && response.Error != null)
					throw new InvalidOperationException(response.Error.Message);
				throw new InvalidOperationException("Request failed");
			}
		}

		public void EditEvent(GoogleCalendarEvent oldEvent, string eventId)
		{
			if (m_Token == null || IcdEnvironment.GetUtcTime() >= m_TokenExpireTime)
				RenewToken();

			string url =
				string.Format("https://www.googleapis.com/calendar/v3/calendars/{0}/events/{1}?access_token={2}",
				              CalendarId, eventId, m_Token);

			string eventData = JsonConvert.SerializeObject(oldEvent, Formatting.None);

			WebPortResponse putResponse = m_Port.Put(url, Encoding.UTF8.GetBytes(eventData));
			if (!putResponse.GotResponse)
				throw new InvalidOperationException("Request failed");

			GoogleCalendarViewResponse response = null;
			if (!string.IsNullOrEmpty(putResponse.DataAsString))
				response = JsonConvert.DeserializeObject<GoogleCalendarViewResponse>(putResponse.DataAsString);
			if (!putResponse.IsSuccessCode)
			{
				if (response != null && response.Error != null)
					throw new InvalidOperationException(response.Error.Message);
				throw new InvalidOperationException("Request failed");
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

		private string RenewToken()
		{
			string payload =
				@"{""iss"": " + ClientEmail + @", ""iat"": " +
				(int)IcdEnvironment.GetUtcTime().ToUnixTimestamp() + @", ""exp"": " +
				(int)(IcdEnvironment.GetUtcTime() + new TimeSpan(0, 30, 0)).ToUnixTimestamp() +
				@", ""aud"": ""https://www.googleapis.com/oauth2/v4/token"", ""scope"": ""https://www.googleapis.com/auth/calendar"", ""sub"": " + CalendarId + @"}";

			// Build the request token
			string privateKey = SanitizePrivateKey(PrivateKey);
			string jwt = JwtUtils.SignRs256(payload, privateKey);

			// Request the OAuth token
			m_Port.Accept = "*/*";
			Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>
			{
				{ "Content-Type", new List<string>{"application/x-www-form-urlencoded"} }
			};

			Dictionary<string, string> body = new Dictionary<string, string>
			{
				{"grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer"},
				{"assertion", jwt}
			};

			byte[] bodyData = HttpUtils.GetFormUrlEncodedContentBytes(body);

			const string url = "https://www.googleapis.com/oauth2/v4/token";

			//Dispatch!
			WebPortResponse output = m_Port.Post(url, headers, bodyData);

			if (!output.IsSuccessCode || !output.GotResponse)
			{
				Logger.Log(eSeverity.Error, "Failed to get token - {0}", output.DataAsString);
				m_Token = null;
				return null;
			}

			//Get the token string value out of the JSON
			GoogleTokenResponse response = JsonConvert.DeserializeObject<GoogleTokenResponse>(output.DataAsString);
			m_Token = response.AccessToken;
			m_TokenExpireTime = IcdEnvironment.GetUtcTime() + new TimeSpan(0, 0, response.ExpiresInSeconds);
			return m_Token;
		}

		public static string SanitizePrivateKey(string privateKey)
		{
			const string privateKeyRegex =
				@"(?'begin'-*BEGIN PRIVATE KEY-*)?(?'key'[^-]+)(?'end'-*END PRIVATE KEY-*(\\n)?)?";

			Match match = Regex.Match(privateKey, privateKeyRegex);
			if (!match.Success)
				throw new FormatException("Private Key does not match expected format");

			return match.Groups["key"].Value.Replace("\n", "");
		}

		/// <summary>
		/// Creates a timestamp string that represents the start of the current day in local time in a format Google accepts.
		/// </summary>
		/// <returns></returns>
		private static string Rfc3339StartOfDayTimeStamp()
		{
			var localTime = IcdEnvironment.GetLocalTime();
			var endOfDay = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0, DateTimeKind.Local);
			return XmlConvert.ToString(endOfDay, XmlDateTimeSerializationMode.Local);
		}

		/// <summary>
		/// Creates a timestamp string that represents the end of the current day in local time in a format Google accepts.
		/// </summary>
		/// <returns></returns>
		private static string Rfc3339EndOfDayTimeStamp()
		{
			var localTime = IcdEnvironment.GetLocalTime();
			var endOfDay = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59, DateTimeKind.Local);
			return XmlConvert.ToString(endOfDay, XmlDateTimeSerializationMode.Local);
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(GoogleCalendarDeviceSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			m_UriProperties.Copy(settings);
			m_WebProxyProperties.Copy(settings);

			ClientEmail = settings.ClientEmail;
			CalendarId = settings.CalendarId;
			PrivateKey = settings.PrivateKey;

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

			ClientEmail = null;
			CalendarId = null;
			PrivateKey = null;
			SetPort(null);

			m_UriProperties.ClearUriProperties();
			m_WebProxyProperties.ClearProxyProperties();
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(GoogleCalendarDeviceSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.CalendarParsingPath = m_CalendarParsingPath;

			settings.ClientEmail = ClientEmail;
			settings.CalendarId = CalendarId;
			settings.PrivateKey = PrivateKey;

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
		protected override void AddControls(GoogleCalendarDeviceSettings settings, IDeviceFactory factory, Action<IDeviceControl> addControl)
		{
			base.AddControls(settings, factory, addControl);

			addControl(new GoogleCalendarControl(this, Controls.Count));
		}

		#endregion

		#region Console

		public override string ConsoleHelp { get { return "The Google service device"; } }

		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;
			yield return new ConsoleCommand("RenewToken", "", () => RenewToken());
		}

		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		#endregion
	}
}
