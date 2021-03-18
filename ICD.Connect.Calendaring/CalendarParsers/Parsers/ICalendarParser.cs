using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.CalendarParsers.Parsers
{
	public interface ICalendarParser
	{
		/// <summary>
		/// Parses text into BookingProtocolInfo collection
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		IDialContext ParseLine(string text);
	}
}
