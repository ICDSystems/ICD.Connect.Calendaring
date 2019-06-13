using System;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring
{
	public sealed class BookingProtocolInfo
    {
	    public eDialProtocol DialProtocol { get; set; }
		public string Number { get; set; }
		public string Password { get; set; }
    }

	public static class BookingProtocolExtensions
	{
		/// <summary>
		/// Converts the booking protocol to the best possible meeting type.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		public static eMeetingType ToMeetingType(this eDialProtocol extends)
		{
			switch (extends)
			{
				case eDialProtocol.Zoom:
				case eDialProtocol.Sip:
					return eMeetingType.VideoConference;

				case eDialProtocol.Pstn:
					return eMeetingType.AudioConference;

				case eDialProtocol.None:
					return eMeetingType.Presentation;

				default:
					throw new ArgumentOutOfRangeException("extends");
			}
		}
	}
}
