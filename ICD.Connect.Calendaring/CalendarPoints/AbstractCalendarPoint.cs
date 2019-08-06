using ICD.Connect.Devices.Points;

namespace ICD.Connect.Calendaring.CalendarPoints
{
	public abstract class AbstractCalendarPoint<TSettings> : AbstractPoint<TSettings>, ICalendarPoint
		where TSettings : ICalendarPointSettings, new()
	{
	}
}
