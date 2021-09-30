using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Connect.Conferencing.DialContexts;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Bookings
{
	public class AbstractBasicBooking : IBooking
	{
		private List<IDialContext> m_DialContexts;

		/// <summary>
		/// Constructor.
		/// </summary>
		public AbstractBasicBooking()
		{
			m_DialContexts = new List<IDialContext>();
		}

		/// <summary>
		/// Returns the name of the meeting.
		/// </summary>
		public string MeetingName { get; set; }

		/// <summary>
		/// Returns the organizer's name.
		/// </summary>
		public string OrganizerName { get; set; }

		/// <summary>
		/// Returns the organizer's email.
		/// </summary>
		public string OrganizerEmail { get; set; }

		/// <summary>
		/// Returns the meeting start time.
		/// </summary>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Returns the meeting end time.
		/// </summary>
		public DateTime EndTime { get; set; }

		/// <summary>
		/// Returns true if meeting is private.
		/// </summary>
		public bool IsPrivate { get; set; }

		/// <summary>
		/// Returns true if the booking is checked in.
		/// </summary>
		public bool CheckedIn { get; set; }

		/// <summary>
		/// Returns true if the booking is checked out.
		/// </summary>
		public bool CheckedOut { get; set; }

		/// <summary>
		/// Returns Booking Numbers.
		/// </summary>
		public IEnumerable<IDialContext> GetBookingNumbers()
		{
			return m_DialContexts.ToArray();
		}

		/// <summary>
		/// Sets the booking numbers.
		/// </summary>
		/// <param name="bookingNumbers"></param>
		public void SetBookingNumbers([NotNull] IEnumerable<IDialContext> bookingNumbers)
		{
			if (bookingNumbers == null)
				throw new ArgumentNullException("bookingNumbers");

			m_DialContexts.Clear();
			m_DialContexts.AddRange(bookingNumbers);
		}
	}

	public abstract class AbstractBasicBookingJsonConverter<T> : AbstractGenericJsonConverter<T> where T:AbstractBasicBooking
	{
		private const string TOKEN_MEETING_NAME = "meetingName";
		private const string TOKEN_ORGANIZER_NAME = "organizerName";
		private const string TOKEN_ORGANIZER_EMAIL = "organizerEmail";
		private const string TOKEN_START_TIME = "startTime";
		private const string TOKEN_END_TIME = "endTime";
		private const string TOKEN_IS_PRIVATE = "isPrivate";
		private const string TOKEN_CHECKED_IN = "checkedIn";
		private const string TOKEN_CHECKED_OUT = "checkedOut";
		private const string TOKEN_DIAL_CONTEXTS = "dialContexts";

		/// <summary>
		/// Override to write properties to the writer.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="serializer"></param>
		protected override void WriteProperties(JsonWriter writer, T value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			if (value.MeetingName != null)
				writer.WriteProperty(TOKEN_MEETING_NAME, value.MeetingName);

			if (value.OrganizerName != null)
				writer.WriteProperty(TOKEN_ORGANIZER_NAME, value.OrganizerName);

			if (value.OrganizerEmail != null)
				writer.WriteProperty(TOKEN_ORGANIZER_EMAIL, value.OrganizerEmail);

			writer.WriteProperty(TOKEN_START_TIME, value.StartTime);
			writer.WriteProperty(TOKEN_END_TIME, value.EndTime);

			if (value.IsPrivate)
				writer.WriteProperty(TOKEN_IS_PRIVATE, true);

			if (value.CheckedIn)
				writer.WriteProperty(TOKEN_CHECKED_IN, true);

			if (value.CheckedOut)
				writer.WriteProperty(TOKEN_CHECKED_OUT, true);

			writer.WritePropertyName(TOKEN_DIAL_CONTEXTS);
			serializer.SerializeArray(writer, value.GetBookingNumbers());
		}

		/// <summary>
		/// Override to handle the current property value with the given name.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="reader"></param>
		/// <param name="instance"></param>
		/// <param name="serializer"></param>
		protected override void ReadProperty(string property, JsonReader reader, T instance, JsonSerializer serializer)
		{
			switch (property)
			{
				case TOKEN_MEETING_NAME:
					instance.MeetingName = reader.GetValueAsString();
					break;

				case TOKEN_ORGANIZER_NAME:
					instance.OrganizerName = reader.GetValueAsString();
					break;

				case TOKEN_ORGANIZER_EMAIL:
					instance.OrganizerEmail = reader.GetValueAsString();
					break;

				case TOKEN_START_TIME:
					instance.StartTime = reader.GetValueAsDateTime();
					break;

				case TOKEN_END_TIME:
					instance.EndTime = reader.GetValueAsDateTime();
					break;

				case TOKEN_IS_PRIVATE:
					instance.IsPrivate = reader.GetValueAsBool();
					break;

				case TOKEN_CHECKED_IN:
					instance.CheckedIn = reader.GetValueAsBool();
					break;

				case TOKEN_CHECKED_OUT:
					instance.CheckedOut = reader.GetValueAsBool();
					break;

				case TOKEN_DIAL_CONTEXTS:
					IEnumerable<IDialContext> bookingNumbers = serializer.DeserializeArray<DialContext>(reader).Cast<IDialContext>();
					instance.SetBookingNumbers(bookingNumbers);
					break;

				default:
					base.ReadProperty(property, reader, instance, serializer);
					return;
			}
		}
	}
}