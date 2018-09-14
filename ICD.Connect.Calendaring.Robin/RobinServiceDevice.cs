using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Robin.Components;
using ICD.Connect.Calendaring.Robin.Controls.Calendar;
using ICD.Connect.Devices;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Protocol.Network.WebPorts;
using ICD.Connect.Settings.Core;

namespace ICD.Connect.Calendaring.Robin
{
	public sealed class RobinServiceDevice : AbstractDevice<RobinServiceDeviceSettings>
	{
	    private IWebPort m_Port;
	    private bool m_Initialized;

        public event EventHandler<BoolEventArgs> OnInitializedChanged;

        #region Properties

        public RobinServiceDeviceComponentFactory Components { get; private set; }
	    public string Token { get; set; }
	    public string ResourceId { get; set; }

        #endregion

        public RobinServiceDevice()
		{
            Components = new RobinServiceDeviceComponentFactory(this);

			Controls.Add(new RobinServiceDeviceCalendarControl(this, Controls.Count));
		}

        #region Methods

	    /// <summary>
	    /// Device Initialized Status.
	    /// </summary>
	    public bool Initialized
	    {
	        get { return m_Initialized; }
	        private set
	        {
	            if (value == m_Initialized)
	                return;

	            m_Initialized = value;

	            OnInitializedChanged.Raise(this, new BoolEventArgs(m_Initialized));
	        }
	    }

        /// <summary>
        /// Sets the port for communication with the service.
        /// </summary>
        /// <param name="port"></param>
        [PublicAPI]
	    public void SetPort(IWebPort port)
	    {
	        if (port == m_Port)
	            return;

	        Unsubscribe(m_Port);

	        if (port != null)
	            port.Accept = "application/json";

	        m_Port = port;
	        Subscribe(m_Port);

	        Initialized = true;
	        UpdateCachedOnlineStatus();
	    }

	    /// <summary>
	    /// gets the port.
	    /// </summary>
	    /// <param name="port"></param>
	    internal IWebPort GetPort()
	    {
            return m_Port;
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

        protected override void ApplySettingsFinal(RobinServiceDeviceSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

		    Token = settings.Token;
		    ResourceId = settings.ResourceId;

            if (settings.Port != null)
			{
				var port = factory.GetOriginatorById<IWebPort>(settings.Port.Value);
				SetPort(port);
			}
		}

	    protected override void ClearSettingsFinal()
	    {
            base.ClearSettingsFinal();

	        Token = null;
	        ResourceId = null;
            SetPort(null);
	    }

	    protected override void CopySettingsFinal(RobinServiceDeviceSettings settings)
	    {
            base.CopySettingsFinal(settings);

	        settings.Token = Token;
	        settings.ResourceId = ResourceId;
            settings.Port = m_Port == null ? (int?)null : m_Port.Id;
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