#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using System;
using ICD.Connect.Calendaring.Google.Responses;
using NUnit.Framework;

namespace ICD.Connect.Calendaring.Google.Tests.Responses
{
	[TestFixture]
	public sealed class GoogleCalendarEventTest
	{
		[Test]
		public void DeserializeTest()
		{
			const string data =
		 @"{
            ""kind"": ""calendar#event"",
            ""etag"": ""\""3116716296338000\"""",
            ""id"": ""7n8tpj21o1u1n19f4k5pcurhuf"",
            ""status"": ""confirmed"",
            ""htmlLink"": ""https://www.google.com/calendar/event?eid=N244dHBqMjFvMXUxbjE5ZjRrNXBjdXJodWYgbGF1cmEuZ29tZXpAcHJvZm91bmQtdGVjaC5jb20"",
            ""created"": ""2019-05-15T15:36:41.000Z"",
            ""updated"": ""2019-05-20T13:15:48.169Z"",
            ""summary"": ""Internship Program Orientation"",
            ""description"": ""\n\n\n\n---------------\nbooked with robin: eyJhY3Rpdml0eSI6Im1lZXRpbmcifQ==\n---------------"",
            ""location"": ""Conference Room, ICDPF PA Office-1-PA Conference Room (8)"",
            ""creator"": {
				""email"": ""darla.mooney@profound-tech.com""

			},
            ""organizer"": {
				""email"": ""darla.mooney@profound-tech.com""

			},
            ""start"": {
				""dateTime"": ""2019-05-20T15:00:00-04:00""

			},
            ""end"": {
				""dateTime"": ""2019-05-20T17:00:00-04:00""

			},
            ""iCalUID"": ""7n8tpj21o1u1n19f4k5pcurhuf@google.com"",
            ""sequence"": 0,
            ""attendees"": [
                {
                    ""email"": ""nicholas.mullin@profound-tech.com"",
                    ""responseStatus"": ""needsAction""

				},
                {
                    ""email"": ""austin.noska@profound-tech.com"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""brooke.pulli@profound-tech.com"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""laura.gomez@profound-tech.com"",
                    ""self"": true,
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""olivia.langan@profound-tech.com"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""profound-tech.com_33343935373332313438@resource.calendar.google.com"",
                    ""displayName"": ""ICDPF PA Office-1-PA Conference Room (8)"",
                    ""resource"": true,
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""darla.mooney@profound-tech.com"",
                    ""organizer"": true,
                    ""responseStatus"": ""accepted""
                }
            ],
            ""reminders"": {
                ""useDefault"": true
            }
        }";

			GoogleCalendarEvent calendarEvent = JsonConvert.DeserializeObject<GoogleCalendarEvent>(data);

			Assert.AreEqual("calendar#event", calendarEvent.Kind);
			Assert.AreEqual("\"3116716296338000\"", calendarEvent.ETag);
			Assert.AreEqual("7n8tpj21o1u1n19f4k5pcurhuf", calendarEvent.Id);
			Assert.AreEqual("confirmed", calendarEvent.Status);
			Assert.AreEqual("https://www.google.com/calendar/event?eid=N244dHBqMjFvMXUxbjE5ZjRrNXBjdXJodWYgbGF1cmEuZ29tZXpAcHJvZm91bmQtdGVjaC5jb20", calendarEvent.HtmlLink);
			Assert.AreEqual(new DateTime(2019, 5, 15, 15, 36, 41), calendarEvent.Created);
			Assert.AreEqual(new DateTime(2019, 5, 20, 13, 15, 48, 169), calendarEvent.Updated);
			Assert.AreEqual("Internship Program Orientation", calendarEvent.Summary);
			Assert.AreEqual("\n\n\n\n---------------\nbooked with robin: eyJhY3Rpdml0eSI6Im1lZXRpbmcifQ==\n---------------", calendarEvent.Description);
			Assert.AreEqual("Conference Room, ICDPF PA Office-1-PA Conference Room (8)",calendarEvent.Location);
			Assert.AreEqual("darla.mooney@profound-tech.com", calendarEvent.Creator.Email);
			Assert.AreEqual("darla.mooney@profound-tech.com", calendarEvent.Organizer.Email);
			Assert.AreEqual(new DateTime(2019, 5, 20, 19, 0, 0), calendarEvent.Start.DateTime);
			Assert.AreEqual(new DateTime(2019, 5, 20, 21, 0, 0), calendarEvent.End.DateTime);
			Assert.AreEqual("7n8tpj21o1u1n19f4k5pcurhuf@google.com", calendarEvent.ICalUid);
			Assert.AreEqual("0", calendarEvent.Sequence);
			Assert.AreEqual(7, calendarEvent.Attendees.Length);
			Assert.AreEqual(true, calendarEvent.Reminders.UseDefault);
		}
	}
}
