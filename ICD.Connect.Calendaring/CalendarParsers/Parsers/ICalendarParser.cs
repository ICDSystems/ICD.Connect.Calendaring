using ICD.Connect.Conferencing.DialContexts;
using System.Collections.Generic;

namespace ICD.Connect.Calendaring.CalendarParsers.Parsers
{
	public interface ICalendarParser
	{
		/// <summary>
		/// Parses text into BookingProtocolInfo collection
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		IEnumerable<IDialContext> ParseText(string text);
	}
}
