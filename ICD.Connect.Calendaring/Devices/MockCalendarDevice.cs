using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Devices;

namespace ICD.Connect.Calendaring.Devices
{
	public sealed class MockCalendarDevice : AbstractDevice<MockCalendarDeviceSettings>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public MockCalendarDevice()
		{
			MockCalendarControl control = new MockCalendarControl(this, 1);
			Controls.Add(control);
		}

		/// <summary>
		/// Gets the current online status of the device.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
			return true;
		}
	}
}