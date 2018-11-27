using System;

namespace ICD.Connect.Calendaring.Booking
{
	public interface ISipBookingNumber : IBookingNumber
	{
		string SipUri { get; }
	}

	public static class SipBookingNumberExtensions
	{
		public static bool IsValidSipUri(this ISipBookingNumber booking)
		{
			Uri uri;
			if (!Uri.TryCreate(booking.SipUri, UriKind.RelativeOrAbsolute, out uri))
				return false;

			return uri.IsWellFormedOriginalString() &&
			       (!uri.IsAbsoluteUri || uri.Scheme.Equals("sip", StringComparison.OrdinalIgnoreCase));
		}
	}
}