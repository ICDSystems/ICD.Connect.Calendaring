using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Devices.Points;

namespace ICD.Connect.Calendaring.CalendarPoints
{
	public interface ICalendarPoint : IPoint<ICalendarControl>
	{
		/// <summary>
		/// Gets/Sets the features for this point.
		/// </summary>
		eCalendarFeatures Features { get; set; }
	}
}
