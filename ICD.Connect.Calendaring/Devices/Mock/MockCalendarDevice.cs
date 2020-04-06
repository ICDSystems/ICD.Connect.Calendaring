using ICD.Connect.Devices.Mock;

namespace ICD.Connect.Calendaring.Devices.Mock
{
	public sealed class MockCalendarDevice : AbstractMockDevice<MockCalendarDeviceSettings>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public MockCalendarDevice()
		{
			MockCalendarControl control = new MockCalendarControl(this, 1);
			Controls.Add(control);
		}
	}
}