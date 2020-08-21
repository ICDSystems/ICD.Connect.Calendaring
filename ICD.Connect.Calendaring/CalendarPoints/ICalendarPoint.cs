using ICD.Common.Properties;
using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Devices.Points;

namespace ICD.Connect.Calendaring.CalendarPoints
{
	public interface ICalendarPoint : IPoint
	{
		/// <summary>
		/// Gets the control for this point.
		/// </summary>
		[CanBeNull]
		new ICalendarControl Control { get; }

		/// <summary>
		/// Gets/Sets the features for this point.
		/// </summary>
		eCalendarFeatures Features { get; set; }
	}
}
