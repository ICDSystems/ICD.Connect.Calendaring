using System;

namespace ICD.Connect.Calendaring.Booking
{
	public interface ISipBooking
	{
		string SipUri { get; }
	}

	public static class SipBookingExtensions
	{
		public static bool IsValidSipUri(this ISipBooking booking)
		{
			Uri uri;
			if (!Uri.TryCreate(booking.SipUri, UriKind.RelativeOrAbsolute, out uri))
				return false;

			return uri.IsWellFormedOriginalString() &&
			       (!uri.IsAbsoluteUri || uri.Scheme.Equals("sip", StringComparison.OrdinalIgnoreCase));
		}
	}
}