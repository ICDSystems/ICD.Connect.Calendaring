using System;
using ICD.Connect.Calendaring.Devices.iCalendar.Parser;
using NUnit.Framework;

namespace ICD.Connect.Calendaring.Tests.Devices.iCalendar.Parser
{
	[TestFixture]
// ReSharper disable InconsistentNaming
	public sealed class iCalendarUtilsTest
// ReSharper restore InconsistentNaming
	{
		
		[Test]
		public void StripOctetLineBreaksTest()
		{
			const string data = @"BEGIN:VEVENT
DTSTART;TZID=America/New_York:20190909T160000
DTEND;TZID=America/New_York:20190909T173000
DTSTAMP:20190708T185214Z
ORGANIZER;CN=booking@profound-tech.com:mailto:booking@profound-tech.com
UID:7rfc7va3dgg80er8mo586q1lac@google.com
ATTENDEE;CUTYPE=RESOURCE;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=ICDPF PA
  Office-1-PA Conference Room (8);X-NUM-GUESTS=0:mailto:profound-tech.com_33
 343935373332313438@resource.calendar.google.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=Reazul
  Hoque;X-NUM-GUESTS=0:mailto:reazul.hoque@icdsystems.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;CN=Ja
 ck Kanarish;X-NUM-GUESTS=0:mailto:jack.kanarish@icdsystems.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=austin
 .noska@profound-tech.com;X-NUM-GUESTS=0:mailto:austin.noska@profound-tech.c
 om
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=Jeffer
 y Thompson;X-NUM-GUESTS=0:mailto:jeffery.thompson@wedoresi.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;CN=Dr
 ew Tingen;X-NUM-GUESTS=0:mailto:drew.tingen@icdsystems.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=Chris 
 Cameron;X-NUM-GUESTS=0:mailto:chris.cameron@icdsystems.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;CN=la
 ura.gomez@profound-tech.com;X-NUM-GUESTS=0:mailto:laura.gomez@profound-tech
 .com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=Brett 
 Heroux;X-NUM-GUESTS=0:mailto:brett.heroux@icdsystems.com
RECURRENCE-ID;TZID=America/New_York:20190909T160000
CREATED:20190627T172330Z
DESCRIPTION:Conference Room at ICDPF PA Office\, Profound Technologies has 
 been reserved using Robin
LAST-MODIFIED:20190708T173244Z
LOCATION:Conference Room\, ICDPF PA Office-1-PA Conference Room (8)
SEQUENCE:0
STATUS:CONFIRMED
SUMMARY:Project Planning
TRANSP:OPAQUE
END:VEVENT";

			const string expected = @"BEGIN:VEVENT
DTSTART;TZID=America/New_York:20190909T160000
DTEND;TZID=America/New_York:20190909T173000
DTSTAMP:20190708T185214Z
ORGANIZER;CN=booking@profound-tech.com:mailto:booking@profound-tech.com
UID:7rfc7va3dgg80er8mo586q1lac@google.com
ATTENDEE;CUTYPE=RESOURCE;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=ICDPF PA Office-1-PA Conference Room (8);X-NUM-GUESTS=0:mailto:profound-tech.com_33343935373332313438@resource.calendar.google.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=Reazul Hoque;X-NUM-GUESTS=0:mailto:reazul.hoque@icdsystems.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;CN=Jack Kanarish;X-NUM-GUESTS=0:mailto:jack.kanarish@icdsystems.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=austin.noska@profound-tech.com;X-NUM-GUESTS=0:mailto:austin.noska@profound-tech.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=Jeffery Thompson;X-NUM-GUESTS=0:mailto:jeffery.thompson@wedoresi.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;CN=Drew Tingen;X-NUM-GUESTS=0:mailto:drew.tingen@icdsystems.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=Chris Cameron;X-NUM-GUESTS=0:mailto:chris.cameron@icdsystems.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;CN=laura.gomez@profound-tech.com;X-NUM-GUESTS=0:mailto:laura.gomez@profound-tech.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=Brett Heroux;X-NUM-GUESTS=0:mailto:brett.heroux@icdsystems.com
RECURRENCE-ID;TZID=America/New_York:20190909T160000
CREATED:20190627T172330Z
DESCRIPTION:Conference Room at ICDPF PA Office\, Profound Technologies has been reserved using Robin
LAST-MODIFIED:20190708T173244Z
LOCATION:Conference Room\, ICDPF PA Office-1-PA Conference Room (8)
SEQUENCE:0
STATUS:CONFIRMED
SUMMARY:Project Planning
TRANSP:OPAQUE
END:VEVENT";

			Assert.AreEqual(expected, iCalendarUtils.StripOctetLineBreaks(data));
		}

		[TestCase("19700308T020000", 1970, 3, 8, 2, 0, 0)]
		[TestCase("20190711T180000Z", 2019, 7, 11, 18, 0, 0)]
		[TestCase("TZID=America/New_York:20190909T160000", 2019, 9, 9, 16, 0, 0)]
		public void ParseDateTimeTest(string data, int year, int month, int day, int hour, int minute, int second)
		{
			DateTime dateTime = iCalendarUtils.ParseDateTime(data);

			Assert.AreEqual(year, dateTime.Year);
			Assert.AreEqual(month, dateTime.Month);
			Assert.AreEqual(day, dateTime.Day);
			Assert.AreEqual(hour, dateTime.Hour);
			Assert.AreEqual(minute, dateTime.Minute);
			Assert.AreEqual(second, dateTime.Second);
		}

		[TestCase(@"Profound Technologies\, 125 Little Conestoga Rd\, Chester Springs\, PA 19425\, USA",
			"Profound Technologies, 125 Little Conestoga Rd, Chester Springs, PA 19425, USA")]
		public void UnescapeValueTest(string data, string expected)
		{
			Assert.AreEqual(expected, iCalendarUtils.UnescapeValue(data));
		}
	}
}
