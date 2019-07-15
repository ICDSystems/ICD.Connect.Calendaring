using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ICD.Connect.Calendaring.Devices.iCalendar.Parser
{
// ReSharper disable InconsistentNaming
	public sealed class iCalendarEvent
// ReSharper restore InconsistentNaming
	{
		private const string EVENT_REGEX = @"(?'key'[^:;]+)[:;](?'value'[^\n]+)?[\n]?";

		private readonly List<iCalendarAttendee> m_Attendees; 

		public DateTime DtStart { get; private set; }
		public DateTime DtEnd { get; private set; }
		public string Rrule { get; private set; }
		public DateTime DtStamp { get; private set; }
		public iCalendarAttendee Organizer { get; private set; }
		public string Uid { get; private set; }
		public IEnumerable<iCalendarAttendee> Attendees { get { return m_Attendees.ToArray(); } }
		public string Class { get; set; }
		public DateTime RecurrenceId { get; private set; } 
		public DateTime Created { get; private set; }
		public string Description { get; private set; }
		public DateTime LastModified { get; private set; }
		public string Location { get; private set; }
		public int Sequence { get; private set; }
		public string Status { get; private set; }
		public string Summary { get; private set; }
		public string Transp { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		private iCalendarEvent()
		{
			m_Attendees = new List<iCalendarAttendee>();
		}

		/// <summary>
		/// Deserializes a new calendar event from the given iCalendar event data.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static iCalendarEvent Deserialize(string data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			data = data.Trim();
			data = iCalendarUtils.StripOctetLineBreaks(data);

			if (!data.StartsWith("BEGIN:VEVENT"))
				throw new FormatException("Missing leading BEGIN:VEVENT");

			if (!data.EndsWith("END:VEVENT"))
				throw new FormatException("Missing trailing END:VEVENT");

			iCalendarEvent output = new iCalendarEvent();

			foreach (string line in data.Split('\n', '\r'))
			{
				Match match = Regex.Match(line, EVENT_REGEX);
				if (!match.Success)
					continue;

				string key = match.Groups["key"].Value;
				string value = match.Groups["value"].Value;

				switch (key)
				{
					case "DTSTART":
						output.DtStart = iCalendarUtils.ParseDateTime(value);
						break;
					case "DTEND":
						output.DtEnd = iCalendarUtils.ParseDateTime(value);
						break;
					case "RRULE":
						output.Rrule = iCalendarUtils.UnescapeValue(value);
						break;
					case "DTSTAMP":
						output.DtStamp = iCalendarUtils.ParseDateTime(value);
						break;
					case "ORGANIZER":
						output.Organizer = iCalendarAttendee.Deserialize(line);
						break;
					case "UID":
						output.Uid = iCalendarUtils.UnescapeValue(value);
						break;
					case "ATTENDEE":
						iCalendarAttendee attendee = iCalendarAttendee.Deserialize(line);
						output.m_Attendees.Add(attendee);
						break;
					case "CLASS":
						output.Class = iCalendarUtils.UnescapeValue(value);
						break;
					case "RECURRENCE-ID":
						output.RecurrenceId = iCalendarUtils.ParseDateTime(value);
						break;
					case "CREATED":
						output.Created = iCalendarUtils.ParseDateTime(value);
						break;
					case "DESCRIPTION":
						output.Description = iCalendarUtils.UnescapeValue(value);
						break;
					case "LAST-MODIFIED":
						output.LastModified = iCalendarUtils.ParseDateTime(value);
						break;
					case "LOCATION":
						output.Location = iCalendarUtils.UnescapeValue(value);
						break;
					case "SEQUENCE":
						output.Sequence = int.Parse(value);
						break;
					case "STATUS":
						output.Status = iCalendarUtils.UnescapeValue(value);
						break;
					case "SUMMARY":
						output.Summary = iCalendarUtils.UnescapeValue(value);
						break;
					case "TRANSP":
						output.Transp = iCalendarUtils.UnescapeValue(value);
						break;
				}
			}

			return output;
		}
	}

}