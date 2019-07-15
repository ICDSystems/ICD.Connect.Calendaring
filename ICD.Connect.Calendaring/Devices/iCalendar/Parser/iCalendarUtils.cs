using System;
using System.Globalization;
using System.Text.RegularExpressions;
using ICD.Common.Utils;

namespace ICD.Connect.Calendaring.Devices.iCalendar.Parser
{
// ReSharper disable InconsistentNaming
	public static class iCalendarUtils
// ReSharper restore InconsistentNaming
	{
		/// <summary>
		/// Removes the line break and space/tab pairs from the given iCalendar data.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string StripOctetLineBreaks(string data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			return data.Replace("\r\n ", "")
			           .Replace("\r\n\t", "");
		}

		/// <summary>
		/// Converts an iCalendar date/time representation to a DateTime.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static DateTime ParseDateTime(string data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			data = data.Trim();

			// TZID=America/New_York:20190909T160000
			const string timeZoneRegex = @"TZID=(?'tzid'[^:]*):(?'datetime'\S+)";

			// Assume the calendar for the room is local to the control system
			// We can't handle timezones by name/id in 2008 :(
			Match match;
			if (RegexUtils.Matches(data, timeZoneRegex, out match))
				data = match.Groups["datetime"].Value;

			// 20190711T180000Z
			if (data.EndsWith("Z"))
				return DateTime.ParseExact(data, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture).ToUniversalTime();

			// 19700308T020000
			return DateTime.ParseExact(data, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Replaces escaped iCalendar values with the original value.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string UnescapeValue(string data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			const string escapeSequenceRegex = @"(?'sequence'\\(?'char'.))";

			return RegexUtils.ReplaceGroup(data, escapeSequenceRegex, "sequence", m =>
			{
				string value = m.Value;

				// Can we unescape it into a newline, tab, etc?
				string unescaped = Regex.Unescape(value);
				if (unescaped != value)
					return unescaped;

				// Just return the second character (e.g. comma)
				return m.Groups["char"].Value;
			});
		}
	}
}