using System;
using System.Linq;
using ICD.Connect.Calendaring.Devices.iCalendar.Parser;
using NUnit.Framework;

namespace ICD.Connect.Calendaring.Tests.Devices.iCalendar.Parser
{
	[TestFixture]
// ReSharper disable InconsistentNaming
	public sealed class iCalendarEventTest
// ReSharper restore InconsistentNaming
	{
		[Test]
		public void DeserializeTest()
		{
			const string data = @"BEGIN:VEVENT
DTSTART;TZID=America/New_York:20190713T143000
DTEND;TZID=America/New_York:20190713T153000
RRULE:FREQ=DAILY
DTSTAMP:20190710T185804Z
ORGANIZER;CN=laura.gomez@profound-tech.com:mailto:laura.gomez@profound-tech
 .com
UID:7pbj08fi2e5prfockjenk58rvh@google.com
ATTENDEE;CUTYPE=RESOURCE;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=ICDPF PA
  Office-2-PA Huddle Room (4);X-NUM-GUESTS=0:mailto:profound-tech.com_393037
 3732373138393839@resource.calendar.google.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;CN=au
 stin.noska@profound-tech.com;X-NUM-GUESTS=0:mailto:austin.noska@profound-te
 ch.com
ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=laura.
 gomez@profound-tech.com;X-NUM-GUESTS=0:mailto:laura.gomez@profound-tech.com
CLASS:PUBLIC
CREATED:20190709T182025Z
DESCRIPTION:This is a TestEvent that we will use to test the test.\nThis Te
 stEvent will help us figure many things out.\nyay!\nyayay!!!
LAST-MODIFIED:20190710T185740Z
LOCATION:Profound Technologies\, 125 Little Conestoga Rd\, Chester Springs\
 , PA 19425\, USA\, ICDPF PA Office-2-PA Huddle Room (4)
SEQUENCE:3
STATUS:CONFIRMED
SUMMARY:TestEvent
TRANSP:TRANSPARENT
END:VEVENT";

			iCalendarEvent calendarEvent = iCalendarEvent.Deserialize(data);

			Assert.AreEqual(new DateTime(2019, 7, 13, 14, 30, 0), calendarEvent.DtStart);
			Assert.AreEqual(new DateTime(2019, 7, 13, 15, 30, 0), calendarEvent.DtEnd);
			Assert.AreEqual("FREQ=DAILY", calendarEvent.Rrule);
			Assert.AreEqual(new DateTime(2019, 7, 10, 18, 58, 04), calendarEvent.DtStamp);
			Assert.AreEqual("7pbj08fi2e5prfockjenk58rvh@google.com", calendarEvent.Uid);
			Assert.AreEqual("PUBLIC", calendarEvent.Class);
			Assert.AreEqual(new DateTime(2019, 7, 9, 18, 20, 25), calendarEvent.Created);
			Assert.AreEqual("This is a TestEvent that we will use to test the test.\nThis TestEvent will help us figure many things out.\nyay!\nyayay!!!", calendarEvent.Description);
			Assert.AreEqual(new DateTime(2019, 7, 10, 18, 57, 40), calendarEvent.LastModified);
			Assert.AreEqual("Profound Technologies, 125 Little Conestoga Rd, Chester Springs, PA 19425, USA, ICDPF PA Office-2-PA Huddle Room (4)", calendarEvent.Location);
			Assert.AreEqual(3, calendarEvent.Sequence);
			Assert.AreEqual("CONFIRMED", calendarEvent.Status);
			Assert.AreEqual("TestEvent",calendarEvent.Summary);
			Assert.AreEqual("TRANSPARENT", calendarEvent.Transp);

			Assert.AreEqual(3, calendarEvent.Attendees.Count());
		}
	}
}
