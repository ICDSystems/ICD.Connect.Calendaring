using System;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.Mock;
using ICD.Connect.Settings;

namespace ICD.Connect.Calendaring.Devices.Mock
{
	public sealed class MockCalendarDevice : AbstractMockDevice<MockCalendarDeviceSettings>
	{
		/// <summary>
		/// Override to add controls to the device.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		/// <param name="addControl"></param>
		protected override void AddControls(MockCalendarDeviceSettings settings, IDeviceFactory factory, Action<IDeviceControl> addControl)
		{
			base.AddControls(settings, factory, addControl);

			addControl(new MockCalendarControl(this, 1));
		}
	}
}