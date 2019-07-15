using System;
using System.Text.RegularExpressions;
using ICD.Common.Utils;

namespace ICD.Connect.Calendaring.Devices.iCalendar.Parser
{
// ReSharper disable InconsistentNaming
	public sealed class iCalendarAttendee
// ReSharper restore InconsistentNaming
	{
		private const string MAILTO_REGEX = @"(?'data':mailto:(?'mailto'[^:]*))";

		public string CuType { get; private set; }
		public string Role { get; private set; }
		public string PartStat { get; private set; }
		public string Cn { get; private set; }
		public int XNumGuests { get; private set; }
		public string XResponseComment { get; private set; }
		public string Mailto { get; private set; }

		/// <summary>
		/// Private constructor.
		/// </summary>
		private iCalendarAttendee()
		{
		}

//("CUTYPE={0};ROLE={1};PARTSTAT={2};CN={3};X-NUM-GUESTS={4}",
		public static iCalendarAttendee Deserialize(string data)
		{
			/*
ATTENDEE;CUTYPE=RESOURCE;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=ICDPF PA
  Office-1-PA Conference Room (8);X-NUM-GUESTS=0:mailto:profound-tech.com_33
 343935373332313438@resource.calendar.google.com
			 */

			if (data == null)
				throw new ArgumentNullException("data");

			data = data.Trim();
			data = iCalendarUtils.StripOctetLineBreaks(data);

			if (!data.StartsWith("ATTENDEE;") && !data.StartsWith("ORGANIZER;"))
				throw new FormatException("Missing leading attendee type");

			iCalendarAttendee output = new iCalendarAttendee();

			// Handle the trailing MAILTO info
			data = RegexUtils.ReplaceGroup(data, MAILTO_REGEX, "data", m =>
			{
				output.Mailto = m.Groups["mailto"].Value;
				return string.Empty;
			});

			// Handle the KVPs
			string[] kvps = data.Split(';');

			foreach (string kvp in kvps)
			{
				string[] split = kvp.Split('=');
				if (split.Length != 2)
					continue;

				string key = split[0];
				string value = split[1];

				switch (key)
				{
					case "CUTYPE":
						output.CuType = iCalendarUtils.UnescapeValue(value);
						break;
					case "ROLE":
						output.Role = iCalendarUtils.UnescapeValue(value);
						break;
					case "PARTSTAT":
						output.PartStat = iCalendarUtils.UnescapeValue(value);
						break;
					case "CN":
						output.Cn = iCalendarUtils.UnescapeValue(value);
						break;
					case "X-NUM-GUESTS":
						output.XNumGuests = int.Parse(value);
						break;
					case "X-RESPONSE-COMMENT":
						output.XResponseComment = iCalendarUtils.UnescapeValue(value).TrimStart('"').TrimEnd('"');
						break;
				}

			}
			return output;
		}
	}
}