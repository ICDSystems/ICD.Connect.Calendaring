using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.Calendaring.CalendarParsers;
using ICD.Connect.Calendaring.Microsoft.Exchange.Controls;
using ICD.Connect.Devices;
using ICD.Connect.Settings;
using Independentsoft.Exchange;
using Independentsoft.Exchange.Autodiscover;

namespace ICD.Connect.Calendaring.Microsoft.Exchange
{
	public sealed class ExchangeCalendarDevice : AbstractDevice<ExchangeCalendarDeviceSettings>
	{
		private readonly CalendarParserCollection m_CalendarParserCollection;

		private string m_CalendarParsingPath;
		private string m_EndPoint;
               
		#region Properties

		public string Username{ get; set; }
		public string Password{ get; set; }
		//public string Endpoint { get; set; }
		public CalendarParserCollection CalendarParserCollection { get { return m_CalendarParserCollection; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public ExchangeCalendarDevice()
		{
			m_CalendarParserCollection = new CalendarParserCollection();

			Controls.Add(new ExchangeCalendarControl(this, Controls.Count));
		}

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

			Username= settings.Username;
			Password = settings.Password;
			//Endpoint = settings.Endpoint;

			SetCalendarParsers(settings.CalendarParsingPath);
		}

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			m_CalendarParserCollection.ClearMatchers();

			Username = null;
			Password = null;
			//Endpoint = null;
		}
		
		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(ExchangeCalendarDeviceSettings settings)
		{
            base.CopySettingsFinal(settings);
			settings.CalendarParsingPath = m_CalendarParsingPath;

			settings.Username = Username;
			settings.Password = Password;
			//settings.Endpoint = Endpoint;           
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

		private string RenewEndPoint()
		{
#if SIMPLSHARP
			AutodiscoverService autodiscoverService =
				new AutodiscoverService("https://outlook.office365.com/Autodiscover/Autodiscover.xml", Username, Password);
#else
			NetworkCredential credential = new NetworkCredential(Username, Password);
			AutodiscoverService autodiscoverService =
				new AutodiscoverService("https://outlook.office365.com/Autodiscover/Autodiscover.xml", credential);
#endif

			List<UserSettingName> settingNames = new List<UserSettingName> {UserSettingName.ExternalEwsUrl};
			GetUserSettingsResponse response = autodiscoverService.GetUserSettings(Username, settingNames);

			UserResponse userResponse = response.UserResponses.FirstOrDefault();
			UserSetting setting = userResponse == null ? null : userResponse.UserSettings.FirstOrDefault(s => s.Name == "ExternalEwsUrl");
			
			m_EndPoint = setting == null ? null : setting.Value;
			return m_EndPoint;
		}

		public IEnumerable<Appointment> GetAppointments()
		{
			if (m_EndPoint == null)
			{
				Log(eSeverity.Error, "Cannot get appointments - no endpoint");
				return Enumerable.Empty<Appointment>();
			}

#if SIMPLSHARP
			Service service = new Service(m_EndPoint, Username, Password);
#else
			NetworkCredential credential = new NetworkCredential(Username, Password);
			Service service = new Service(m_EndPoint, credential);
#endif

			FindItemResponse response = null;

			try
			{
				CalendarView view = new CalendarView(DateTime.Today, DateTime.Today.AddMonths(1));
				response = service.FindItem(StandardFolder.Calendar, AppointmentPropertyPath.AllPropertyPaths, view);
			}
			catch (Exception ex)
			{
				Log(eSeverity.Error, "Failed to get response - {0}", ex.Message);
			}

			return response == null
				? Enumerable.Empty<Appointment>()
				: response.Items.OfType<Appointment>();
		}
	
		private IEnumerable<IConsoleCommand>  GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

#endregion
	}
}
