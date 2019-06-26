using ICD.Connect.Calendaring.Microsoft.Office365.Responses;
using ICD.Connect.Protocol.EventArguments;
using Independentsoft.Email.Mime;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ICD.Connect.Calendaring.Microsoft.Tests.Office365.Converters
{
	[TestFixture]
	public sealed class CalendarViewResponseConverterTest
	{
		[Test]
		public void DeserializeTest()
		{
			const string json = @"{
    ""@odata.context"": ""https://graph.microsoft.com/v1.0/$metadata#users('629d7bad-e44b-4ee4-891b-45e4fc6b254a')/calendars('AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8-aT1LccS5Py6q2XXxmQAAAAAAEGAAAX8-aT1LccS5Py6q2XXxmQAAAAABE4AAA%3D')/calendarView"",
			""value"": [
		        {
            ""@odata.etag"": ""W/\""F/P2k9S3HEuT8uqtl18ZkAAACQsHDg==\"""",
		            ""id"": ""AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8-aT1LccS5Py6q2XXxmQAAAAAAENAAAX8-aT1LccS5Py6q2XXxmQAAAJDkbRAAA="",
		            ""createdDateTime"": ""2019-06-25T20:50:54.4848023Z"",
		            ""lastModifiedDateTime"": ""2019-06-25T20:50:54.8096139Z"",
		            ""changeKey"": ""F/P2k9S3HEuT8uqtl18ZkAAACQsHDg=="",
		            ""categories"": [],
            ""originalStartTimeZone"": ""UTC"",
            ""originalEndTimeZone"": ""UTC"",
            ""iCalUId"": ""040000008200E00074C5B7101A82E00800000000B7797EAE972BD501000000000000000010000000F433F8266AA6B3469E48B01A606EF0C5"",
            ""reminderMinutesBeforeStart"": 15,
            ""isReminderOn"": false,
            ""hasAttachments"": false,
            ""subject"": ""alright"",
            ""bodyPreview"": """",
            ""importance"": ""normal"",
            ""sensitivity"": ""normal"",
            ""isAllDay"": true,
            ""isCancelled"": false,
            ""isOrganizer"": true,
            ""responseRequested"": true,
            ""seriesMasterId"": null,
            ""showAs"": ""free"",
            ""type"": ""singleInstance"",
            ""webLink"": ""https://outlook.office365.com/owa/?itemid=AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8%2FaT1LccS5Py6q2XXxmQAAAAAAENAAAX8%2FaT1LccS5Py6q2XXxmQAAAJDkbRAAA%3D&exvsurl=1&path=/calendar/item"",
            ""onlineMeetingUrl"": null,
            ""recurrence"": null,
            ""responseStatus"": {
                ""response"": ""organizer"",
                ""time"": ""0001-01-01T00:00:00Z""
            },
            ""body"": {
                ""contentType"": ""html"",
                ""content"": ""<html>\r\n<head>\r\n<meta http-equiv=\""Content-Type\"" content=\""text/html; charset=utf-8\"">\r\n<meta content=\""text/html; charset=us-ascii\"">\r\n<style type=\""text/css\"" style=\""display:none\"">\r\n<!--\r\np\r\n\t{margin-top:0;\r\n\tmargin-bottom:0}\r\n-->\r\n</style>\r\n</head>\r\n<body dir=\""ltr\"">\r\n<div id=\""divtagdefaultwrapper\"" dir=\""ltr\"" style=\""font-size:12pt; color:#000000; font-family:Calibri,Helvetica,sans-serif\"">\r\n<p style=\""margin-top:0; margin-bottom:0\""><br>\r\n</p>\r\n</div>\r\n</body>\r\n</html>\r\n""
            },
            ""start"": {
                ""dateTime"": ""2019-06-25T00:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""end"": {
                ""dateTime"": ""2019-06-26T00:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""location"": {
                ""displayName"": ""room"",
                ""locationType"": ""default"",
                ""uniqueId"": ""room"",
                ""uniqueIdType"": ""private""
            },
            ""locations"": [
                {
                    ""displayName"": ""room"",
                    ""locationType"": ""default"",
                    ""uniqueId"": ""room"",
                    ""uniqueIdType"": ""private""

				}
            ],
            ""attendees"": [],
            ""organizer"": {
                ""emailAddress"": {
                    ""name"": ""Interns"",
                    ""address"": ""interns@profoundtech.onmicrosoft.com""
                }
            }
        },
        {
            ""@odata.etag"": ""W/\""F/P2k9S3HEuT8uqtl18ZkAAACQsIOg==\"""",
            ""id"": ""AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8-aT1LccS5Py6q2XXxmQAAAAAAENAAAX8-aT1LccS5Py6q2XXxmQAAAJDkbSAAA="",
            ""createdDateTime"": ""2019-06-25T20:51:00.7671532Z"",
            ""lastModifiedDateTime"": ""2019-06-25T21:24:05.8414505Z"",
            ""changeKey"": ""F/P2k9S3HEuT8uqtl18ZkAAACQsIOg=="",
            ""categories"": [],
            ""originalStartTimeZone"": ""UTC"",
            ""originalEndTimeZone"": ""UTC"",
            ""iCalUId"": ""040000008200E00074C5B7101A82E00800000000F4C73CB2972BD501000000000000000010000000CA1203C0A73029439B7E6EDDAA1862A0"",
            ""reminderMinutesBeforeStart"": 15,
            ""isReminderOn"": false,
            ""hasAttachments"": false,
            ""subject"": ""kk"",
            ""bodyPreview"": """",
            ""importance"": ""normal"",
            ""sensitivity"": ""normal"",
            ""isAllDay"": true,
            ""isCancelled"": false,
            ""isOrganizer"": true,
            ""responseRequested"": true,
            ""seriesMasterId"": null,
            ""showAs"": ""free"",
            ""type"": ""singleInstance"",
            ""webLink"": ""https://outlook.office365.com/owa/?itemid=AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8%2FaT1LccS5Py6q2XXxmQAAAAAAENAAAX8%2FaT1LccS5Py6q2XXxmQAAAJDkbSAAA%3D&exvsurl=1&path=/calendar/item"",
            ""onlineMeetingUrl"": null,
            ""recurrence"": null,
            ""responseStatus"": {
                ""response"": ""organizer"",
                ""time"": ""0001-01-01T00:00:00Z""
            },
            ""body"": {
                ""contentType"": ""html"",
                ""content"": ""<html>\r\n<head>\r\n<meta http-equiv=\""Content-Type\"" content=\""text/html; charset=utf-8\"">\r\n<meta content=\""text/html; charset=us-ascii\"">\r\n<style type=\""text/css\"" style=\""display:none\"">\r\n<!--\r\np\r\n\t{margin-top:0;\r\n\tmargin-bottom:0}\r\n-->\r\n</style>\r\n</head>\r\n<body dir=\""ltr\"">\r\n<div id=\""divtagdefaultwrapper\"" dir=\""ltr\"" style=\""font-size:12pt; color:#000000; font-family:Calibri,Helvetica,sans-serif\"">\r\n<p style=\""margin-top:0; margin-bottom:0\""><br>\r\n</p>\r\n</div>\r\n</body>\r\n</html>\r\n""
            },
            ""start"": {
                ""dateTime"": ""2019-06-25T00:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""end"": {
                ""dateTime"": ""2019-06-26T00:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""location"": {
                ""displayName"": """",
                ""locationType"": ""default"",
                ""uniqueIdType"": ""unknown"",
                ""address"": {},
                ""coordinates"": {}
            },
            ""locations"": [],
            ""attendees"": [],
            ""organizer"": {
                ""emailAddress"": {
                    ""name"": ""Interns"",
                    ""address"": ""interns@profoundtech.onmicrosoft.com""
                }
            }
        }
    ]
}";

			CalendarViewResponse response = JsonConvert.DeserializeObject<CalendarViewResponse>(json);

			Assert.AreEqual(2, response.Value.Length);
		}
	}
}
