#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Connect.Calendaring.Google.Responses;
using NUnit.Framework;

namespace ICD.Connect.Calendaring.Google.Tests.Responses
{
	[TestFixture]
	public sealed class GoogleCalendarAttendeeTest
	{
		[TestCase(@"{""email"": ""nicholas.mullin@profound-tech.com"", ""responseStatus"": ""needsAction""}", "nicholas.mullin@profound-tech.com", null, false, false, "needsAction")]
		public void DeserializeTest(string data, string email, string displayName, bool organizer, bool resource, string status)
		{
			//const string test = ;
				/*{
					""email"": ""austin.noska @profound-tech.com"",
					""responseStatus"": ""needsAction""
				},
				{
					""email"": ""brooke.pulli @profound-tech.com"",
					""responseStatus"": ""needsAction""
				},
				{
					""email"": ""laura.gomez @profound-tech.com"",
					""self"": true,
					""responseStatus"": ""needsAction""
				},
				{
					""email"": ""olivia.langan @profound-tech.com"",
					""responseStatus"": ""needsAction""
				},
				{
					""email"": ""profound-tech.com_33343935373332313438 @resource.calendar.google.com"",
					""displayName"": ""ICDPF PA Office-1-PA Conference Room (8)"",
					""resource"": true,
					""responseStatus"": ""accepted""
				},
				{
					""email"": ""darla.mooney @profound-tech.com"",
					""organizer"": true,
					""responseStatus"": ""accepted""
				}
				]}";
				*/

			GoogleCalendarEventAttendee attendee = JsonConvert.DeserializeObject<GoogleCalendarEventAttendee>(data);

			Assert.AreEqual(email, attendee.Email);
			Assert.AreEqual(status, attendee.ResponseStatus);
		}
	}
}
