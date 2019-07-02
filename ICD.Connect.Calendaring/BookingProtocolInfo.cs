using System;
using ICD.Connect.Calendaring.Booking;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring
{
	public struct BookingProtocolInfo : IEquatable<BookingProtocolInfo>
    {
	    public eDialProtocol DialProtocol { get; set; }
		public string Number { get; set; }
		public string Password { get; set; }

	    public bool Equals(BookingProtocolInfo other)
	    {
		    return DialProtocol == other.DialProtocol &&
		           string.Equals(Number, other.Number) &&
		           string.Equals(Password, other.Password);
	    }

	    public override bool Equals(object obj)
	    {
		    if (ReferenceEquals(null, obj))
			    return false;
		    return obj is BookingProtocolInfo && Equals((BookingProtocolInfo)obj);
	    }

	    public override int GetHashCode()
	    {
		    unchecked
		    {
			    int hashCode = (int)DialProtocol;
			    hashCode = (hashCode * 397) ^ (Number != null ? Number.GetHashCode() : 0);
			    hashCode = (hashCode * 397) ^ (Password != null ? Password.GetHashCode() : 0);
			    return hashCode;
		    }
	    }
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
