using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Devices.Points;
using ICD.Connect.Settings;

namespace ICD.Connect.Calendaring.CalendarPoints
{
	public abstract class AbstractCalendarPoint<TSettings> : AbstractPoint<TSettings, ICalendarControl>, ICalendarPoint
		where TSettings : ICalendarPointSettings, new()
	{
		/// <summary>
		/// Gets the category for this originator type (e.g. Device, Port, etc)
		/// </summary>
		public override string Category { get { return "CalendarPoint"; } }

		/// <summary>
		/// Gets/Sets the features for this point.
		/// </summary>
		public eCalendarFeatures Features { get; set; }

		#region Settings

		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			Features = settings.Features;
		}

		protected override void CopySettingsFinal(TSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.Features = Features;
		}

		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			Features = eCalendarFeatures.None;
		}

		#endregion
	}
}
