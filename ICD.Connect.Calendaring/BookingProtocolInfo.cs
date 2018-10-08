using System;
using ICD.Connect.Calendaring.Booking;

namespace ICD.Connect.Calendaring
{
	public sealed class BookingProtocolInfo
    {
	    public eBookingProtocol BookingProtocol { get; set; }
		public string Number { get; set; }
    }

	/// <summary>
	/// The types of protocols for booking numbers.
	/// Arranged in ascending order of least qualified to most qualified.
	/// </summary>
	public enum eBookingProtocol
	{
		None = 0,
		Pstn = 1,
		Sip = 2,
		Zoom = 3
	}

	public static class BookingProtocolExtensions
	{
		/// <summary>
		/// Converts the booking protocol to the best possible meeting type.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		public static eMeetingType ToMeetingType(this eBookingProtocol extends)
		{
			switch (extends)
			{
				case eBookingProtocol.Zoom:
				case eBookingProtocol.Sip:
					return eMeetingType.VideoConference;

				case eBookingProtocol.Pstn:
					return eMeetingType.AudioConference;

				case eBookingProtocol.None:
					return eMeetingType.Presentation;

				default:
					throw new ArgumentOutOfRangeException("extends");
			}
		}
	}
}
