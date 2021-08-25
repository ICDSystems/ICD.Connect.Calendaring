#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using ICD.Connect.Calendaring.Robin.Components.Converters;
using ICD.Connect.Calendaring.Robin.Components.Events;
using NUnit.Framework;

namespace ICD.Connect.Calendaring.Robin.Tests.Components.Converters
{
	[TestFixture]
	public sealed class EventConverterTest
	{
		private EventConverter m_Test;

		[SetUp]
		public void Setup()
		{
			m_Test = new EventConverter();
		}

		[Test]
		public void CanConvert_ReturnsTrueForEventType()
		{
			m_Test.CanConvert(typeof(Event));
		}

		[Test]
		public void DeserializeTest()
		{
			Event ev = JsonConvert.DeserializeObject<Event>(TEST_EVENT);
			Assert.That(ev, Is.Not.Null);
			Assert.That(ev.MeetingStart, Is.Not.Null);
			Assert.That(ev.MeetingEnd, Is.Not.Null);
			Assert.That(ev.OrganizerEmail, Is.EqualTo("drew.tingen@icdsystems.com"));
		}
		
		private const string TEST_EVENT =
@"{
    ""id"": ""96347456_20190521T143000Z"",
    ""organizer_id"": null,
    ""organizer_email"": ""booking@profound-tech.com"",
    ""creator_id"": 166888,
    ""creator_email"": ""drew.tingen@icdsystems.com"",
    ""space_id"": 56313,
    ""title"": ""Programmers Standup"",
    ""description"": ""----------------\n  Zoom Meeting Link: https://zoom.us/j/467047195\n  \n  Dial In Numbers:\n  US East: +1 305 859 3847, +1 646 558 8656\n  US West: +1 408 638 0968, +1 415 762 9988\n  Global: https://zoom.us/zoomconference\n  Meeting ID: 467047195\n  ----------------"",
    ""location"": ""Huddle Room, ICDPF PA Office-2-PA Huddle Room (4)"",
    ""remote_calendar_mailbox_address"": ""profound-tech.com_3930373732373138393839@resource.calendar.google.com"",
    ""remote_event_id"": null,
    ""remote_type"": ""google"",
    ""creation_type"": ""manual"",
    ""uid"": ""644e9lv5at5hdnae5ulb01nk50@google.com"",
    ""started_at"": ""2019-05-21T10:30:00-04:00"",
    ""ended_at"": ""2019-05-21T10:50:00-04:00"",
    ""start"": {
      ""date_time"": ""2019-05-21T10:30:00-04:00"",
      ""time_zone"": ""America/New_York""
    },
    ""end"": {
      ""date_time"": ""2019-05-21T10:50:00-04:00"",
      ""time_zone"": ""America/New_York""
    },
    ""series_id"": ""96347456"",
    ""recurrence_id"": ""20190521T143000Z"",
    ""status"": ""confirmed"",
    ""recurrence"": null,
    ""is_all_day"": false,
    ""visibility"": ""default"",
    ""show_as"": ""busy"",
    ""invitees"": [
      {
        ""id"": ""434571872"",
        ""event_id"": ""96347456_20190521T143000Z"",
        ""user_id"": 166475,
        ""space_id"": null,
        ""email"": ""chris.cameron@icdsystems.com"",
        ""display_name"": ""Chris Cameron"",
        ""response_status"": ""not_responded"",
        ""participation_role"": ""required"",
        ""is_organizer"": false,
        ""is_resource"": false,
        ""created_at"": ""2019-03-12T11:07:34-04:00"",
        ""updated_at"": ""2019-03-12T11:07:34-04:00""
      },
      {
        ""id"": ""434571877"",
        ""event_id"": ""96347456_20190521T143000Z"",
        ""user_id"": null,
        ""space_id"": null,
        ""email"": ""jeffery.thompson@icdsystems.com"",
        ""display_name"": ""Jeffery Thompson"",
        ""response_status"": ""accepted"",
        ""participation_role"": ""required"",
        ""is_organizer"": false,
        ""is_resource"": false,
        ""created_at"": ""2019-03-12T11:07:34-04:00"",
        ""updated_at"": ""2019-05-08T11:39:51-04:00""
      }
    ],
    ""confirmation"": null
  }
";
	}
}
