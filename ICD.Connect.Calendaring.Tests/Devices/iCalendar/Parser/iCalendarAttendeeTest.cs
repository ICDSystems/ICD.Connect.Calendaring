using ICD.Connect.Calendaring.Devices.iCalendar.Parser;
using NUnit.Framework;

namespace ICD.Connect.Calendaring.Tests.Devices.iCalendar.Parser
{
	[TestFixture]
// ReSharper disable InconsistentNaming
	public sealed class iCalendarAttendeeTest
// ReSharper restore InconsistentNaming
	{
		[TestCase(
			@"ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=DECLINED;CN=olivia.langan@profound-tech.com;X-NUM-GUESTS=0;X-RESPONSE-COMMENT=""Declined because I am out of office"":mailto:olivia.langan@profound-tech.com",
			"INDIVIDUAL", "REQ-PARTICIPANT", "DECLINED", "olivia.langan@profound-tech.com", 0, "olivia.langan@profound-tech.com",
			"Declined because I am out of office")]
		[TestCase(
			@"ATTENDEE;CUTYPE=RESOURCE;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;CN=ICDPF PAOffice-2-PA Huddle Room (4);X-NUM-GUESTS=0:mailto:profound-tech.com_3930373732373138393839@resource.calendar.google.com",
			"RESOURCE", "REQ-PARTICIPANT", "ACCEPTED", "ICDPF PAOffice-2-PA Huddle Room (4)", 0,
			"profound-tech.com_3930373732373138393839@resource.calendar.google.com",
			null)]
		[TestCase(@"ORGANIZER;CN=darla.mooney@profound-tech.com:mailto:darla.mooney@profound-tech.com",
			null, null, null, "darla.mooney@profound-tech.com", 0, "darla.mooney@profound-tech.com", null)]
		public void DeserializeTest(string data, string cuType, string role, string partStat, string cn, int guestCount,
		                            string mailto, string responseComment)
		{
			iCalendarAttendee calendarAttendee = iCalendarAttendee.Deserialize(data);

			Assert.AreEqual(cuType, calendarAttendee.CuType);
			Assert.AreEqual(role, calendarAttendee.Role);
			Assert.AreEqual(partStat, calendarAttendee.PartStat);
			Assert.AreEqual(cn, calendarAttendee.Cn);
			Assert.AreEqual(guestCount, calendarAttendee.XNumGuests);
			Assert.AreEqual(mailto, calendarAttendee.Mailto);
			Assert.AreEqual(responseComment, calendarAttendee.XResponseComment);
		}
	}
}
