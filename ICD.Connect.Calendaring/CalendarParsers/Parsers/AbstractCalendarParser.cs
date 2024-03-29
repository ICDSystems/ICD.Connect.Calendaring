﻿using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.CalendarParsers.Parsers
{
	public abstract class AbstractCalendarParser : ICalendarParser
	{
		/// <summary>
		/// Parses text into BookingProtocolInfo collection
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public abstract IDialContext ParseLine(string text);
	}
}
