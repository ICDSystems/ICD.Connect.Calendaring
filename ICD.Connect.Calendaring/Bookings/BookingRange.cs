﻿using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Bookings
{
	[JsonConverter(typeof(BookingRangeJsonConverter))]
	public sealed class BookingRange
	{
		private readonly List<Booking> m_Bookings;

		public DateTime From { get; set; }
		public DateTime To { get; set; }

		public IEnumerable<Booking> Bookings
		{
			get { return m_Bookings.ToArray(); }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				IEnumerable<Booking> ordered =
					value.OrderBy(b => b.StartTime)
					     .ThenBy(b => b.EndTime);

				m_Bookings.SetRange(ordered);
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public BookingRange()
		{
			m_Bookings = new List<Booking>();
		}
	}

	public sealed class BookingRangeJsonConverter : AbstractGenericJsonConverter<BookingRange>
	{
		private const string ATTRIBUTE_FROM = "from";
		private const string ATTRIBUTE_TO = "to";
		private const string ATTRIBUTE_BOOKINGS = "bookings";

		/// <summary>
		/// Override to write properties to the writer.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="serializer"></param>
		protected override void WriteProperties(JsonWriter writer, BookingRange value, JsonSerializer serializer)
		{
			base.WriteProperties(writer, value, serializer);

			writer.WriteProperty(ATTRIBUTE_FROM, value.From);
			writer.WriteProperty(ATTRIBUTE_TO, value.To);
			
			writer.WritePropertyName(ATTRIBUTE_BOOKINGS);
			serializer.SerializeArray(writer, value.Bookings);
		}

		/// <summary>
		/// Override to handle the current property value with the given name.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="reader"></param>
		/// <param name="instance"></param>
		/// <param name="serializer"></param>
		protected override void ReadProperty(string property, JsonReader reader, BookingRange instance,
		                                     JsonSerializer serializer)
		{
			switch (property)
			{
				case ATTRIBUTE_FROM:
					instance.From = reader.GetValueAsDateTime();
					break;

				case ATTRIBUTE_TO:
					instance.To = reader.GetValueAsDateTime();
					break;

				case ATTRIBUTE_BOOKINGS:
					instance.Bookings = serializer.DeserializeArray<Booking>(reader);
					break;

				default:
					base.ReadProperty(property, reader, instance, serializer);
					break;
			}
		}
	}
}