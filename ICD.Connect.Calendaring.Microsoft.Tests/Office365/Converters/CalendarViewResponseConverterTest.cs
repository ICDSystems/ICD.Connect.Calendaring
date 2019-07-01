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
    ""@odata.context"": ""https://graph.microsoft.com/v1.0/$metadata#users('629d7bad-e44b-4ee4-891b-45e4fc6b254a')/events"",
			""value"": [
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
        },
        {
            ""@odata.etag"": ""W/\""F/P2k9S3HEuT8uqtl18ZkAAACh+flg==\"""",
            ""id"": ""AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8-aT1LccS5Py6q2XXxmQAAAAAAENAAAX8-aT1LccS5Py6q2XXxmQAAAJDkbRAAA="",
            ""createdDateTime"": ""2019-06-25T20:50:54.4848023Z"",
            ""lastModifiedDateTime"": ""2019-06-27T07:03:53.2996959Z"",
            ""changeKey"": ""F/P2k9S3HEuT8uqtl18ZkAAACh+flg=="",
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
                ""uniqueId"": ""3505f2c0-3e0d-4f59-8cb0-8549c359c3ec"",
                ""uniqueIdType"": ""locationStore""
            },
            ""locations"": [
                {
                    ""displayName"": ""room"",
                    ""locationType"": ""default"",
                    ""uniqueId"": ""3505f2c0-3e0d-4f59-8cb0-8549c359c3ec"",
                    ""uniqueIdType"": ""locationStore""
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
            ""@odata.etag"": ""W/\""F/P2k9S3HEuT8uqtl18ZkAAACh+flw==\"""",
            ""id"": ""AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8-aT1LccS5Py6q2XXxmQAAAAAAENAAAX8-aT1LccS5Py6q2XXxmQAAAJDkbQAAA="",
            ""createdDateTime"": ""2019-06-25T19:58:44.1798341Z"",
            ""lastModifiedDateTime"": ""2019-06-27T07:03:53.389644Z"",
            ""changeKey"": ""F/P2k9S3HEuT8uqtl18ZkAAACh+flw=="",
            ""categories"": [],
            ""originalStartTimeZone"": ""UTC"",
            ""originalEndTimeZone"": ""UTC"",
            ""iCalUId"": ""040000008200E00074C5B7101A82E00800000000EAFEAF64902BD5010000000000000000100000003BEF44B7BA2F78419775003538C513B8"",
            ""reminderMinutesBeforeStart"": 15,
            ""isReminderOn"": false,
            ""hasAttachments"": false,
            ""subject"": ""yayyy"",
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
            ""webLink"": ""https://outlook.office365.com/owa/?itemid=AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8%2FaT1LccS5Py6q2XXxmQAAAAAAENAAAX8%2FaT1LccS5Py6q2XXxmQAAAJDkbQAAA%3D&exvsurl=1&path=/calendar/item"",
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
                ""dateTime"": ""2019-06-26T00:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""end"": {
                ""dateTime"": ""2019-06-27T00:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""location"": {
                ""displayName"": ""here"",
                ""locationType"": ""default"",
                ""uniqueId"": ""06745efc-a27e-483a-8894-756af4864416"",
                ""uniqueIdType"": ""locationStore""
            },
            ""locations"": [
                {
                    ""displayName"": ""here"",
                    ""locationType"": ""default"",
                    ""uniqueId"": ""06745efc-a27e-483a-8894-756af4864416"",
                    ""uniqueIdType"": ""locationStore""
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
            ""@odata.etag"": ""W/\""F/P2k9S3HEuT8uqtl18ZkAAABu9Z2w==\"""",
            ""id"": ""AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8-aT1LccS5Py6q2XXxmQAAAAAAENAAAX8-aT1LccS5Py6q2XXxmQAAAGH3KNAAA="",
            ""createdDateTime"": ""2019-06-20T17:31:34.3187143Z"",
            ""lastModifiedDateTime"": ""2019-06-23T22:23:51.2430303Z"",
            ""changeKey"": ""F/P2k9S3HEuT8uqtl18ZkAAABu9Z2w=="",
            ""categories"": [],
            ""originalStartTimeZone"": ""Pacific Standard Time"",
            ""originalEndTimeZone"": ""Pacific Standard Time"",
            ""iCalUId"": ""040000008200E00074C5B7101A82E00800000000EE359D018E27D50100000000000000001000000068CF61F913311640AED5E00F3DEF30E0"",
            ""reminderMinutesBeforeStart"": 15,
            ""isReminderOn"": false,
            ""hasAttachments"": false,
            ""subject"": ""calender test"",
            ""bodyPreview"": """",
            ""importance"": ""normal"",
            ""sensitivity"": ""normal"",
            ""isAllDay"": false,
            ""isCancelled"": false,
            ""isOrganizer"": true,
            ""responseRequested"": true,
            ""seriesMasterId"": null,
            ""showAs"": ""busy"",
            ""type"": ""singleInstance"",
            ""webLink"": ""https://outlook.office365.com/owa/?itemid=AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8%2FaT1LccS5Py6q2XXxmQAAAAAAENAAAX8%2FaT1LccS5Py6q2XXxmQAAAGH3KNAAA%3D&exvsurl=1&path=/calendar/item"",
            ""onlineMeetingUrl"": null,
            ""recurrence"": null,
            ""responseStatus"": {
                ""response"": ""organizer"",
                ""time"": ""0001-01-01T00:00:00Z""
            },
            ""body"": {
                ""contentType"": ""html"",
                ""content"": """"
            },
            ""start"": {
                ""dateTime"": ""2019-06-21T12:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""end"": {
                ""dateTime"": ""2019-06-21T12:30:00.0000000"",
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
        },
        {
            ""@odata.etag"": ""W/\""F/P2k9S3HEuT8uqtl18ZkAAABu9Z3w==\"""",
            ""id"": ""AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8-aT1LccS5Py6q2XXxmQAAAAAAENAAAX8-aT1LccS5Py6q2XXxmQAAAFekEYAAA="",
            ""createdDateTime"": ""2019-06-19T20:07:18.3385649Z"",
            ""lastModifiedDateTime"": ""2019-06-23T22:23:53.4017864Z"",
            ""changeKey"": ""F/P2k9S3HEuT8uqtl18ZkAAABu9Z3w=="",
            ""categories"": [],
            ""originalStartTimeZone"": ""Pacific Standard Time"",
            ""originalEndTimeZone"": ""Pacific Standard Time"",
            ""iCalUId"": ""040000008200E00074C5B7101A82E008000000004396AB98DA26D501000000000000000010000000BDCC211D65865A4293217B3C706E04F3"",
            ""reminderMinutesBeforeStart"": 15,
            ""isReminderOn"": false,
            ""hasAttachments"": false,
            ""subject"": ""hello"",
            ""bodyPreview"": """",
            ""importance"": ""normal"",
            ""sensitivity"": ""normal"",
            ""isAllDay"": false,
            ""isCancelled"": false,
            ""isOrganizer"": true,
            ""responseRequested"": true,
            ""seriesMasterId"": null,
            ""showAs"": ""busy"",
            ""type"": ""singleInstance"",
            ""webLink"": ""https://outlook.office365.com/owa/?itemid=AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8%2FaT1LccS5Py6q2XXxmQAAAAAAENAAAX8%2FaT1LccS5Py6q2XXxmQAAAFekEYAAA%3D&exvsurl=1&path=/calendar/item"",
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
                ""dateTime"": ""2019-06-13T12:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""end"": {
                ""dateTime"": ""2019-06-13T12:30:00.0000000"",
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
        },
        {
            ""@odata.etag"": ""W/\""F/P2k9S3HEuT8uqtl18ZkAAACwtlbA==\"""",
            ""id"": ""AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8-aT1LccS5Py6q2XXxmQAAAAAAENAAAX8-aT1LccS5Py6q2XXxmQAAAAADhGAAA="",
            ""createdDateTime"": ""2019-06-10T20:28:30.6320084Z"",
            ""lastModifiedDateTime"": ""2019-06-27T21:12:27.8211196Z"",
            ""changeKey"": ""F/P2k9S3HEuT8uqtl18ZkAAACwtlbA=="",
            ""categories"": [
                ""Orange"",
                ""Purple""
            ],
            ""originalStartTimeZone"": ""Pacific Standard Time"",
            ""originalEndTimeZone"": ""Pacific Standard Time"",
            ""iCalUId"": ""040000008200E00074C5B7101A82E0080000000011054D11CB1FD50100000000000000001000000030C72BEE97B40243B52A08B1D6B979DD"",
            ""reminderMinutesBeforeStart"": 15,
            ""isReminderOn"": false,
            ""hasAttachments"": false,
            ""subject"": ""Test Event"",
            ""bodyPreview"": """",
            ""importance"": ""normal"",
            ""sensitivity"": ""normal"",
            ""isAllDay"": false,
            ""isCancelled"": false,
            ""isOrganizer"": true,
            ""responseRequested"": true,
            ""seriesMasterId"": null,
            ""showAs"": ""busy"",
            ""type"": ""singleInstance"",
            ""webLink"": ""https://outlook.office365.com/owa/?itemid=AAMkAGQzZDk5YmY4LWY4NDktNDQ4YS1hMjk2LTE5YWQzMzg1Y2UzMwBGAAAAAAByHJNZ4c7wTInZUDGNyoCSBwAX8%2FaT1LccS5Py6q2XXxmQAAAAAAENAAAX8%2FaT1LccS5Py6q2XXxmQAAAAADhGAAA%3D&exvsurl=1&path=/calendar/item"",
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
                ""dateTime"": ""2019-06-12T19:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""end"": {
                ""dateTime"": ""2019-06-12T22:00:00.0000000"",
                ""timeZone"": ""UTC""
            },
            ""location"": {
                ""displayName"": ""Upstairs"",
                ""locationType"": ""default"",
                ""uniqueId"": ""befd8c97-56c5-4c70-b192-173c0d73078d"",
                ""uniqueIdType"": ""locationStore""
            },
            ""locations"": [
                {
                    ""displayName"": ""Upstairs"",
                    ""locationType"": ""default"",
                    ""uniqueId"": ""befd8c97-56c5-4c70-b192-173c0d73078d"",
                    ""uniqueIdType"": ""locationStore""
                }
            ],
            ""attendees"": [
                {
                    ""type"": ""required"",
                    ""status"": {
                        ""response"": ""accepted"",
                        ""time"": ""2019-06-27T20:57:36Z""
                    },
                    ""emailAddress"": {
                        ""name"": ""lv.gomez@outlook.com"",
                        ""address"": ""lv.gomez@outlook.com""
                    }
                },
                {
                    ""type"": ""required"",
                    ""status"": {
                        ""response"": ""declined"",
                        ""time"": ""2019-06-27T21:01:20.5784127Z""
                    },
                    ""emailAddress"": {
                        ""name"": ""ConfRoom"",
                        ""address"": ""confroom@profoundtech.onmicrosoft.com""
                    }
                }
            ],
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

			Assert.AreEqual(6, response.Value.Length);
		}
	}
}
