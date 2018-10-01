using System;
using System.Collections.Generic;

namespace ICD.Connect.Calendaring.CalendarParsers.Parsers
{
	public abstract class AbstractCalendarParser : ICalendarParser
	{
		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractCalendarParser()
		{
		}

		#endregion

		public abstract IEnumerable<BookingProtocolInfo> ParseText(string text);
	}
}
