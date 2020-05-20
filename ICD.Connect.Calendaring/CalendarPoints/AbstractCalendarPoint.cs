using ICD.Connect.Calendaring.Controls;
using ICD.Connect.Devices.Points;

namespace ICD.Connect.Calendaring.CalendarPoints
{
	public abstract class AbstractCalendarPoint<TSettings> : AbstractPoint<TSettings, ICalendarControl>, ICalendarPoint
		where TSettings : ICalendarPointSettings, new()
	{
		/// <summary>
		/// Gets the category for this originator type (e.g. Device, Port, etc)
		/// </summary>
		public override string Category { get { return "CalendarPoint"; } }
	}
}
