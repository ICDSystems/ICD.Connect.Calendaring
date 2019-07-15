using ICD.Connect.Devices;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Calendaring.Devices.Mock
{
	[KrangSettings("MockCalendarDevice", typeof(MockCalendarDevice))]
	public sealed class MockCalendarDeviceSettings : AbstractDeviceSettings
	{
	}
}
