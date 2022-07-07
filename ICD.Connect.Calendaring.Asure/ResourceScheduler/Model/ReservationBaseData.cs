using System;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils.Xml;

namespace ICD.Connect.Calendaring.Asure.ResourceScheduler.Model
{
	public sealed class ReservationBaseData : AbstractData
	{
		public const string ELEMENT = "ReservationBaseData";

		#region Properties

		[PublicAPI]
		public int Id { get; private set; }

		[PublicAPI]
		public string Description { get; private set; }

		[PublicAPI]
		public string Notes { get; private set; }

		[PublicAPI]
		public ReservationAttendeeData[] ReservationAttendees { get; private set; }

		[PublicAPI]
		public ReservationResourceData[] ReservationResources { get; private set; }

		[PublicAPI]
		public int? CreatedByUserId { get; private set; }

		[PublicAPI]
		public int? CreatedForUserId { get; private set; }

		[PublicAPI]
		[CanBeNull]
		public ReservationAttendeeData CreatedByAttendee { get; private set; }

		[PublicAPI]
		[CanBeNull]
		public ReservationAttendeeData CreatedForAttendee { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a ReservationBaseData instance from xml.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static ReservationBaseData FromXml(string xml)
		{
			ReservationBaseData output = new ReservationBaseData
			{
				Id = XmlUtils.ReadChildElementContentAsInt(xml, "Id"),
				Description = XmlUtils.ReadChildElementContentAsString(xml, "Description"),
				Notes = XmlUtils.ReadChildElementContentAsString(xml, "Notes")
			};

			string attendeesXml = XmlUtils.GetChildElementAsString(xml, "Attendees");

			output.ReservationAttendees = XmlUtils
			                              .GetChildElementsAsString(attendeesXml, ReservationAttendeeData.ELEMENT)
			                              .Select(x => ReservationAttendeeData.FromXml(x))
			                              .ToArray();

			var attendees = output.ReservationAttendees.Where(a => a.Id != 0).ToDictionary(a => a.Id);

			string resourcesXml = XmlUtils.GetChildElementAsString(xml, "Resources");
			output.ReservationResources = XmlUtils
			                              .GetChildElementsAsString(resourcesXml, ReservationResourceData.ELEMENT)
			                              .Select(x => ReservationResourceData.FromXml(x))
			                              .ToArray();

			int? createdByUserId = XmlUtils.TryReadChildElementContentAsInt(xml, "CreatedByUserId");
			if (createdByUserId.HasValue && createdByUserId.Value != 0)
			{
				output.CreatedByUserId = createdByUserId;
				ReservationAttendeeData createdByAttendee;
				if (attendees.TryGetValue(createdByUserId.Value, out createdByAttendee))
					output.CreatedByAttendee = createdByAttendee;
			}

			int? createdForUserId = XmlUtils.TryReadChildElementContentAsInt(xml, "CreatedForUserId");
			if (createdForUserId.HasValue && createdForUserId.Value != 0)
			{
				output.CreatedForUserId = createdForUserId;
				ReservationAttendeeData createdForAttendee;
				if (attendees.TryGetValue(createdForUserId.Value, out createdForAttendee))
					output.CreatedForAttendee = createdForAttendee;
			}

			return output;
		}

		#endregion
	}
}
