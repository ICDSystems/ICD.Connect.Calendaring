using System.Collections.Generic;
#if !SIMPLSHARP
using System.Net;
#endif
using System.Linq;
using ICD.Common.Utils;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices;
using ICD.Connect.Settings;
using Independentsoft.Exchange.Autodiscover;

namespace ICD.Connect.Calendaring.Microsoft.Exchange
{
	public sealed class ExchangeCalendarDevice : AbstractDevice<ExchangeCalendarDeviceSettings>
	{
		#region Properties

		public string Username { get; set; }

		public string Password { get; set; }

		public string Url { get; set; }

		#endregion

		/// <summary>
		/// Gets the current online status of the device.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
			return true;
		}

		#region Settings

		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			Username = null;
			Password = null;
			Url = null;
		}

		protected override void CopySettingsFinal(ExchangeCalendarDeviceSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.Username = Username;
			settings.Password = Password;
			settings.Url = Url;
		}

		protected override void ApplySettingsFinal(ExchangeCalendarDeviceSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			Username = settings.Username;
			Password = settings.Password;
			Url = settings.Url;
		}

		#endregion

		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("Username", Username);
			addRow("Password", StringUtils.PasswordFormat(Password));
			addRow("Url", Url);
		}

		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			yield return new ConsoleCommand("Test", "", () => TestCalendar());
		}

		private void TestCalendar()
		{
#if SIMPLSHARP
AutodiscoverService autodiscoverService = new AutodiscoverService("https://outlook.office365.com/Autodiscover/Autodiscover.xml", "interns@profoundtech.onmicrosoft.com", "internFTW2019#");
#else
			ICredentials credential = new NetworkCredential("interns@profoundtech.onmicrosoft.com", "internFTW2019#");
			AutodiscoverService autodiscoverService = new AutodiscoverService("https://outlook.office365.com/Autodiscover/Autodiscover.xml", credential);
#endif
			List<UserSettingName> settingNames = new List<UserSettingName> {UserSettingName.ExternalEwsUrl};
			GetUserSettingsResponse response = autodiscoverService.GetUserSettings("interns@profoundtech.onmicrosoft.com", settingNames);

			string url = response.UserResponses.First().UserSettings.FirstOrDefault(s => s.Name == "ExternalEwsUrl").Value;
			IcdConsole.PrintLine(eConsoleColor.Magenta, url);
		}

		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}
	}
}