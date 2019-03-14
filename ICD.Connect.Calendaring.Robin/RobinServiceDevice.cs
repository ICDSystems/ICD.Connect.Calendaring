using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Calendaring.CalendarParsers;
using ICD.Connect.Calendaring.Robin.Components;
using ICD.Connect.Calendaring.Robin.Controls.Calendar;
using ICD.Connect.Devices;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Protocol.Extensions;
using ICD.Connect.Protocol.Network.Ports.Web;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Settings;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin
{
	public sealed class RobinServiceDevice : AbstractDevice<RobinServiceDeviceSettings>
	{
		/// <summary>
		/// Raises the new port that has been assigned.
		/// </summary>
		public event EventHandler<GenericEventArgs<IWebPort>> OnSetPort;

		private readonly UriProperties m_UriProperties;

		private readonly CalendarParserCollection m_CalendarParserCollection;

		private readonly IDictionary<string, List<string>> m_Headers =
			new Dictionary<string, List<string>>
			{
				{"Connection", new List<string> {"keep-alive"}}
			};

		private string m_CalendarParsingPath;
		private IWebPort m_Port;

		#region Properties

		public RobinServiceDeviceComponentFactory Components { get; private set; }

		public string Token
		{
			get
			{
				List<string> values;
				return m_Headers.TryGetValue("Authorization", out values) ? values.FirstOrDefault() : null;
			}
			set
			{
				List<string> values;
				if (!m_Headers.TryGetValue("Authorization", out values))
				{
					values = new List<string>();
					m_Headers.Add("Authorization", values);
				}

				values.Clear();
				values.Add(value);
			}
		}

		public string ResourceId { get; set; }

		public CalendarParserCollection CalendarParserCollection { get { return m_CalendarParserCollection; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public RobinServiceDevice()
		{
			m_UriProperties = new UriProperties();

			m_CalendarParserCollection = new CalendarParserCollection();

			Components = new RobinServiceDeviceComponentFactory(this);

			Controls.Add(new RobinServiceDeviceCalendarControl(this, Controls.Count));
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			OnSetPort = null;

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

			OnSetPort.Raise(this, new GenericEventArgs<IWebPort>(m_Port));
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

		public string Request(string path)
		{
			if (m_Port == null)
				throw new InvalidOperationException("Failed to make request - Port is null");

			bool success;
			string response;

			try
			{
				success = m_Port.Get(path, m_Headers, out response);
			}
			// Catch HTTP or HTTPS exception, without dependency on Crestron
			catch (Exception e)
			{
				string message = string.Format("Failed to make request - {0}", e.Message);
				throw new InvalidOperationException(message, e);
			}

			if (!success)
			{
				string message = string.Format("Request did not succeed");
				throw new InvalidOperationException(message);
			}

			if (string.IsNullOrEmpty(response))
			{
				string message = string.Format("Failed to make request - Received empty response");
				throw new InvalidOperationException(message);
			}

			return JsonConvert.DeserializeObject<Dictionary<string, object>>(response)["data"].ToString();
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
				Log(eSeverity.Error, "failed to load Calendar Parsers {0} - {1}", configPath, e.Message);
			}
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(RobinServiceDeviceSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			m_UriProperties.Copy(settings);

			Token = settings.Token;
			ResourceId = settings.ResourceId;

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

			m_CalendarParserCollection.ClearMatchers();

			Token = null;
			ResourceId = null;
			SetPort(null);

			m_UriProperties.Clear();
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(RobinServiceDeviceSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.CalendarParsingPath = m_CalendarParsingPath;

			settings.Token = Token;
			settings.ResourceId = ResourceId;
			settings.Port = m_Port == null ? (int?)null : m_Port.Id;

			settings.Copy(m_UriProperties);
		}

		#endregion

		#region Console

		/// <summary>
		/// Gets the help information for the node.
		/// </summary>
		public override string ConsoleHelp { get { return "The Robin service device"; } }

		#endregion
	}
}
