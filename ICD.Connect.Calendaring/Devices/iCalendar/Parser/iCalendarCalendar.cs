using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ICD.Connect.Calendaring.Devices.iCalendar.Parser
{
// ReSharper disable InconsistentNaming
	public sealed class iCalendarCalendar
// ReSharper restore InconsistentNaming
	{
		private const string EVENT_REGEX = @"(?'event'BEGIN:VEVENT[\s\S]*?END:VEVENT)";

		private readonly List<iCalendarEvent> m_Events;

		/// <summary>
		/// Constructor.
		/// </summary>
		private iCalendarCalendar()
		{
			m_Events = new List<iCalendarEvent>();
		}

		/// <summary>
		/// Gets the events in the calendar.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<iCalendarEvent> GetEvents()
		{
			return m_Events.ToArray();
		}

		/// <summary>
		/// Deserializes a new calendar instance from the given iCalendar data.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static iCalendarCalendar Deserialize(string data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			data = data.Trim();
			data = iCalendarUtils.StripOctetLineBreaks(data);

			if (!data.StartsWith("BEGIN:VCALENDAR"))
				throw new FormatException("Missing leading BEGIN:VCALENDAR");

			if (!data.EndsWith("END:VCALENDAR"))
				throw new FormatException("Missing trailing END:VCALENDAR");

			iCalendarCalendar output = new iCalendarCalendar();

			// Read the events
			foreach (Match match in Regex.Matches(data, EVENT_REGEX))
			{
				iCalendarEvent iCalEvent = iCalendarEvent.Deserialize(match.Value);
				output.m_Events.Add(iCalEvent);
			}

			return output;
		}
	}
}