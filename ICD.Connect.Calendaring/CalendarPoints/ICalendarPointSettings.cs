using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Devices.Points;

namespace ICD.Connect.Calendaring.CalendarPoints
{
	public interface ICalendarPointSettings: IPointSettings
	{
		eCalendarFeatures Features { get; set; }
	}
}
