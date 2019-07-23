using ICD.Connect.Calendaring.Google.Responses;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ICD.Connect.Calendaring.Google.Tests
{
	[TestFixture]
	public sealed class GoogleCalendarViewConverterTest
	{
		[Test]
		public void DeserializeTest()
		{
			const string test = @"{
    ""kind"": ""calendar#events"",
    ""etag"": ""\""p330974kpteue60g\"""",
    ""summary"": ""laura.gomez@profound-tech.com"",
    ""updated"": ""2019-07-17T20:54:31.064Z"",
    ""timeZone"": ""America/New_York"",
    ""accessRole"": ""reader"",
    ""defaultReminders"": [],
    ""nextSyncToken"": ""CMCTkpnrvOMCEMCTkpnrvOMCGAE="",
    ""items"": [
        {
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
        },
        {
            ""kind"": ""calendar#event"",
            ""etag"": ""\""3116718298144000\"""",
            ""id"": ""1e46p2973ou0rmr19t1d66f42q"",
            ""status"": ""confirmed"",
            ""htmlLink"": ""https://www.google.com/calendar/event?eid=MWU0NnAyOTczb3Uwcm1yMTl0MWQ2NmY0MnEgbGF1cmEuZ29tZXpAcHJvZm91bmQtdGVjaC5jb20"",
            ""created"": ""2019-05-14T18:07:04.000Z"",
            ""updated"": ""2019-05-20T13:32:29.072Z"",
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
                ""dateTime"": ""2019-05-20T09:30:00-04:00""
            },
            ""end"": {
                ""dateTime"": ""2019-05-20T11:30:00-04:00""
            },
            ""iCalUID"": ""1e46p2973ou0rmr19t1d66f42q@google.com"",
            ""sequence"": 1,
            ""attendees"": [
                {
                    ""email"": ""profound-tech.com_33343935373332313438@resource.calendar.google.com"",
                    ""displayName"": ""ICDPF PA Office-1-PA Conference Room (8)"",
                    ""resource"": true,
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""jeffery.thompson@wedoresi.com"",
                    ""optional"": true,
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""chris.cameron@wedoresi.com"",
                    ""optional"": true,
                    ""responseStatus"": ""tentative""
                },
                {
                    ""email"": ""jack.kanarish@wedoresi.com"",
                    ""optional"": true,
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""olivia.langan@profound-tech.com"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""darla.mooney@profound-tech.com"",
                    ""organizer"": true,
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""kevin.busza@profound-tech.com"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""laura.gomez@profound-tech.com"",
                    ""self"": true,
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""nicholas.mullin@profound-tech.com"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""austin.noska@profound-tech.com"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""sandra.martinez@profound-tech.com"",
                    ""displayName"": ""Sandra Martinez"",
                    ""optional"": true,
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""brett.fisher@wedoresi.com"",
                    ""displayName"": ""Brett Fisher"",
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""brooke.pulli@profound-tech.com"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""karen.kean@profound-tech.com"",
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""tina.delong@profound-tech.com"",
                    ""optional"": true,
                    ""responseStatus"": ""needsAction""
                }
            ],
            ""reminders"": {
                ""useDefault"": true
            }
        },
        {
            ""kind"": ""calendar#event"",
            ""etag"": ""\""3116907085514000\"""",
            ""id"": ""paraojbppl8cdtk8jsv68otb00"",
            ""status"": ""confirmed"",
            ""htmlLink"": ""https://www.google.com/calendar/event?eid=cGFyYW9qYnBwbDhjZHRrOGpzdjY4b3RiMDAgbGF1cmEuZ29tZXpAcHJvZm91bmQtdGVjaC5jb20"",
            ""created"": ""2019-05-21T15:40:32.000Z"",
            ""updated"": ""2019-05-21T15:45:42.757Z"",
            ""summary"": ""Sprint Planning"",
            ""description"": ""Conference Room at ICDPF PA Office, Profound Technologies has been reserved using Robin"",
            ""location"": ""Conference Room, ICDPF PA Office-1-PA Conference Room (8)"",
            ""creator"": {
                ""email"": ""booking@profound-tech.com""
            },
            ""organizer"": {
                ""email"": ""booking@profound-tech.com""
            },
            ""start"": {
                ""dateTime"": ""2019-05-21T15:00:00-04:00""
            },
            ""end"": {
                ""dateTime"": ""2019-05-21T16:30:00-04:00""
            },
            ""iCalUID"": ""paraojbppl8cdtk8jsv68otb00@google.com"",
            ""sequence"": 0,
            ""attendees"": [
                {
                    ""email"": ""austin.noska@icdsystems.com"",
                    ""displayName"": """",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""laura.gomez@profound-tech.com"",
                    ""self"": true,
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""jack.kanarish@icdsystems.com"",
                    ""displayName"": ""Jack Kanarish"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""reazul.hoque@icdsystems.com"",
                    ""displayName"": """",
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""jeffery.thompson@wedoresi.com"",
                    ""displayName"": ""Jeffery Thompson"",
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""profound-tech.com_33343935373332313438@resource.calendar.google.com"",
                    ""displayName"": ""ICDPF PA Office-1-PA Conference Room (8)"",
                    ""resource"": true,
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""chris.cameron@icdsystems.com"",
                    ""displayName"": ""Chris Cameron"",
                    ""responseStatus"": ""accepted""
                }
            ],
            ""extendedProperties"": {
                ""shared"": {
                    ""com.robinpowered.event.creator_id"": ""166475"",
                    ""com.robinpowered.event.creation_type"": ""2""
                }
            },
            ""guestsCanModify"": true,
            ""reminders"": {
                ""useDefault"": true
            }
        },
        {
            ""kind"": ""calendar#event"",
            ""etag"": ""\""3116907694082000\"""",
            ""id"": ""b54jsjenqdu4nrcod4rs78jfn0"",
            ""status"": ""confirmed"",
            ""htmlLink"": ""https://www.google.com/calendar/event?eid=YjU0anNqZW5xZHU0bnJjb2Q0cnM3OGpmbjAgbGF1cmEuZ29tZXpAcHJvZm91bmQtdGVjaC5jb20"",
            ""created"": ""2019-05-21T14:57:21.000Z"",
            ""updated"": ""2019-05-21T15:50:47.041Z"",
            ""summary"": ""Programmers Introductions"",
            ""description"": ""Conference Room at ICDPF PA Office, Profound Technologies has been reserved using Robin"",
            ""location"": ""Conference Room, ICDPF PA Office-1-PA Conference Room (8)"",
            ""creator"": {
                ""email"": ""booking@profound-tech.com""
            },
            ""organizer"": {
                ""email"": ""booking@profound-tech.com""
            },
            ""start"": {
                ""dateTime"": ""2019-05-21T16:30:00-04:00""
            },
            ""end"": {
                ""dateTime"": ""2019-05-21T17:00:00-04:00""
            },
            ""iCalUID"": ""b54jsjenqdu4nrcod4rs78jfn0@google.com"",
            ""sequence"": 0,
            ""attendees"": [
                {
                    ""email"": ""profound-tech.com_33343935373332313438@resource.calendar.google.com"",
                    ""displayName"": ""ICDPF PA Office-1-PA Conference Room (8)"",
                    ""resource"": true,
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""jack.kanarish@icdsystems.com"",
                    ""displayName"": ""Jack Kanarish"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""jeffery.thompson@wedoresi.com"",
                    ""displayName"": ""Jeffery Thompson"",
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""greg.gaskill@icdsystems.com"",
                    ""displayName"": ""Greg Gaskill"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""laura.gomez@profound-tech.com"",
                    ""self"": true,
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""chris.cameron@icdsystems.com"",
                    ""displayName"": ""Chris Cameron"",
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""brett.heroux@icdsystems.com"",
                    ""displayName"": ""Brett Heroux"",
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""brett.fisher@wedoresi.com"",
                    ""displayName"": ""Brett Fisher"",
                    ""responseStatus"": ""tentative""
                },
                {
                    ""email"": ""tom.stokes@profound-tech.com"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""reazul.hoque@icdsystems.com"",
                    ""displayName"": """",
                    ""responseStatus"": ""accepted""
                },
                {
                    ""email"": ""brett.fisher@profound-tech.com"",
                    ""displayName"": ""Brett Fisher"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""drew.tingen@icdsystems.com"",
                    ""displayName"": ""Drew Tingen"",
                    ""responseStatus"": ""needsAction""
                },
                {
                    ""email"": ""austin.noska@profound-tech.com"",
                    ""responseStatus"": ""accepted""
                }
            ],
            ""extendedProperties"": {
                ""shared"": {
                    ""com.robinpowered.event.creator_id"": ""166475"",
                    ""com.robinpowered.event.creation_type"": ""2""
                }
            },
            ""guestsCanModify"": true,
            ""reminders"": {
                ""useDefault"": true
            }
        }
    ]
}";

			GoogleCalendarViewResponse response = JsonConvert.DeserializeObject<GoogleCalendarViewResponse>(test);

			Assert.AreEqual(4, response.Items.Length);
		}
	}
}
