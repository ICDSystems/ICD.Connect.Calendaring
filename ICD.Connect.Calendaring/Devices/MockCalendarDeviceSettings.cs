using ICD.Connect.Devices;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Calendaring.Devices
{
	[KrangSettings("MockCalendarDevice", typeof(MockCalendarDevice))]
	public sealed class MockCalendarDeviceSettings : AbstractDeviceSettings
	{
	}
}
