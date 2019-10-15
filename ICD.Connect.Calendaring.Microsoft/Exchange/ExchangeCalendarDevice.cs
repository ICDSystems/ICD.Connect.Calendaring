﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ICD.Common.Properties;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.Calendaring.CalendarParsers;
using ICD.Connect.Calendaring.Microsoft.Exchange.Controls;
using ICD.Connect.Devices;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Settings;
using Independentsoft.Exchange;
using Independentsoft.Exchange.Autodiscover;

namespace ICD.Connect.Calendaring.Microsoft.Exchange
{
	public sealed class ExchangeCalendarDevice : AbstractDevice<ExchangeCalendarDeviceSettings>
	{
		private readonly CalendarParserCollection m_CalendarParserCollection;

		private readonly UriProperties m_UriProperties;
		private readonly WebProxyProperties m_WebProxyProperties;

		private string m_CalendarParsingPath;
		private string m_EndPoint;

		#region Properties

		public CalendarParserCollection CalendarParserCollection { get { return m_CalendarParserCollection; } }

		private string Username { get { return m_UriProperties.UriUsername; } }

		private string Password { get { return m_UriProperties.UriPassword; } }

		private Service Service
		{
			get
			{
				string endpoint = m_EndPoint ?? RenewEndPoint();

#if SIMPLSHARP
				Service service = new Service(endpoint, Username, Password);
#else
				NetworkCredential credential = new NetworkCredential(Username, Password);
				Service service = new Service(endpoint, credential);
#endif

				ConfigureProxy(service.Proxy);

				return service;
			}
		}

		private AutodiscoverService AutodiscoverService
		{
			get
			{
				string uri = m_UriProperties.GetAddress();

#if SIMPLSHARP
				AutodiscoverService service = new AutodiscoverService(uri, Username, Password);
#else
				NetworkCredential credential = new NetworkCredential(Username, Password);
				AutodiscoverService service = new AutodiscoverService(uri, credential);
#endif

				ConfigureProxy(service.Proxy);

				return service;
			}
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public ExchangeCalendarDevice()
		{
			m_UriProperties = new UriProperties();
			m_WebProxyProperties = new WebProxyProperties();

			m_CalendarParserCollection = new CalendarParserCollection();

			Controls.Add(new ExchangeCalendarControl(this, Controls.Count));
		}

		#region Methods

		/// <summary>
		/// Uses the autodiscover service to update the URL used for further EWS requests.
		/// </summary>
		/// <returns></returns>
		[CanBeNull]
		public string RenewEndPoint()
		{
			List<UserSettingName> settingNames = new List<UserSettingName> {UserSettingName.ExternalEwsUrl};
			GetUserSettingsResponse response = AutodiscoverService.GetUserSettings(Username, settingNames);

			UserResponse userResponse = response.UserResponses.FirstOrDefault();
			UserSetting setting =
				userResponse == null
					? null
					: userResponse.UserSettings.FirstOrDefault(s => s.Name == "ExternalEwsUrl");

			m_EndPoint = setting == null ? null : setting.Value;
			return m_EndPoint;
		}

		/// <summary>
		/// Gets the appointments for today.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Appointment> GetAppointments()
		{
			CalendarView view = new CalendarView(DateTime.Today, DateTime.Today.AddDays(1));

			try
			{
				return Service.FindItem(StandardFolder.Calendar, AppointmentPropertyPath.AllPropertyPaths, view)
				              .Items
				              .OfType<Appointment>();
			}
			catch (Exception ex)
			{
				Log(eSeverity.Error, "Failed to get response - {0}", ex.Message);
				return Enumerable.Empty<Appointment>();
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
			return true;
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
				Log(eSeverity.Error, "Failed to load Calendar Parsers - {0}", e.Message);
			}
		}

		private void ConfigureProxy(WebProxy proxy)
		{
			if (proxy == null)
				throw new ArgumentNullException("proxy");

			proxy.Address = m_WebProxyProperties.GetUri();
			proxy.Credentials = new NetworkCredential(m_WebProxyProperties.ProxyUsername, m_WebProxyProperties.ProxyPassword);
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(ExchangeCalendarDeviceSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			m_UriProperties.Copy(settings);
			m_WebProxyProperties.Copy(settings);

			SetCalendarParsers(settings.CalendarParsingPath);
		}

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			m_UriProperties.ClearUriProperties();
			m_WebProxyProperties.ClearProxyProperties();

			m_CalendarParserCollection.ClearMatchers();
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(ExchangeCalendarDeviceSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.Copy(m_UriProperties);
			settings.Copy(m_WebProxyProperties);

			settings.CalendarParsingPath = m_CalendarParsingPath;
		}

		#endregion

		#region Console

		public override string ConsoleHelp { get { return "The Exchange service device"; } }

		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			yield return new ConsoleCommand("RenewEndPoint", "", () => RenewEndPoint());
		}

		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		#endregion
	}
}
