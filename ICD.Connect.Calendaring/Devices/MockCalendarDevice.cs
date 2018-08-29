using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Devices;

namespace ICD.Connect.Calendaring.Devices
{
	public class MockCalendarDevice : AbstractDevice<MockCalendarDeviceSettings>
	{
		public MockCalendarDevice()
		{
			MockCalendarControl control = new MockCalendarControl(this, 1);
			Controls.Add(control);
		}

		protected override bool GetIsOnlineStatus()
		{
			return true;
		}
	}
}