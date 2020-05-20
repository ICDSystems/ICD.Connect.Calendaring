using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Conferencing.DialContexts;
using ICD.Connect.Conferencing.EventArguments;

namespace ICD.Connect.Calendaring
{
	public static class ConferencingBookingUtils
	{
		/// <summary>
		/// Gets the best dialer for the booking based on what is supported by the given dialers.
		/// </summary>
		/// <param name="booking"></param>
		/// <param name="dialers"></param>
		/// <param name="number"></param>
		/// <returns></returns>
		[CanBeNull]
		public static IConferenceDeviceControl GetBestDialer(IBooking booking, IEnumerable<IConferenceDeviceControl> dialers,
		                                                  out IDialContext number)
		{
			if (booking == null)
				throw new ArgumentNullException("booking");

			if (dialers == null)
				throw new ArgumentNullException("dialers");

			// Build map of dialer to best number
			Dictionary<IConferenceDeviceControl, IDialContext> map = dialers.ToDictionary(d => d, d => GetBestNumber(d, booking));

			IConferenceDeviceControl output = map.Keys
			                                  .Where(d => map.GetDefault(d) != null)
			                                  .OrderByDescending(d => d.CanDial(map[d]))
			                                  .ThenByDescending(d => d.Supports)
			                                  .FirstOrDefault();

			number = output == null ? null : map.GetDefault(output);

			return output;
		}

		/// <summary>
		/// Gets the meeting type for the booking based on what is supported by the given dialers.
		/// </summary>
		/// <param name="booking"></param>
		/// <param name="dialers"></param>
		/// <returns></returns>
		public static eMeetingType GetMeetingType(IBooking booking, IEnumerable<IConferenceDeviceControl> dialers)
		{
			if (booking == null)
				throw new ArgumentNullException("booking");

			if (dialers == null)
				throw new ArgumentNullException("dialers");

			IDialContext dialContext;
			IConferenceDeviceControl preferredDialer = GetBestDialer(booking, dialers, out dialContext);

			if (preferredDialer == null)
				return eMeetingType.Presentation;

			eMeetingType meetingType = GetMeetingType(dialContext.Protocol);

			// Get the intersection of the supported conference source types against the booking source types
			eCallType supported = preferredDialer.Supports;
			eCallType meetingSourceType = GetConferenceSourceType(meetingType);
			eCallType intersection = EnumUtils.GetFlagsIntersection(supported, meetingSourceType);

			// Convert back to meeting type
			return GetMeetingType(intersection);
		}

		/// <summary>
		/// Converts the conference source type to a meeting type.
		/// </summary>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		private static eMeetingType GetMeetingType(eCallType sourceType)
		{
			if (sourceType.HasFlag(eCallType.Video))
				return eMeetingType.VideoConference;
			if (sourceType.HasFlag(eCallType.Audio))
				return eMeetingType.AudioConference;

			return eMeetingType.Presentation;
		}

		/// <summary>
		/// Returns the booking number from the given booking that is best supported by the given dialer.
		/// </summary>
		/// <param name="dialer"></param>
		/// <param name="booking"></param>
		/// <returns></returns>
		[CanBeNull]
		public static IDialContext GetBestNumber(IConferenceDeviceControl dialer, IBooking booking)
		{
			if (dialer == null)
				throw new ArgumentNullException("dialer");

			if (booking == null)
				throw new ArgumentNullException("booking");

			return booking.GetBookingNumbers()
			              .OrderByDescending(n => dialer.CanDial(n))
			              .FirstOrDefault();
		}

		public static eCallType GetConferenceSourceType(eDialProtocol protocol)
		{
			return GetConferenceSourceType(GetMeetingType(protocol));
		}

		public static eCallType GetConferenceSourceType(eMeetingType meetingType)
		{
			switch (meetingType)
			{
				case eMeetingType.AudioConference:
					return eCallType.Audio;
				case eMeetingType.VideoConference:
					return eCallType.Audio | eCallType.Video;
				case eMeetingType.Presentation:
					return eCallType.Unknown;

				default:
					throw new ArgumentOutOfRangeException("meetingType");
			}
		}
		
		/// <summary>
		/// Converts the booking protocol to the best possible meeting type.
		/// </summary>
		/// <param name="protocol"></param>
		/// <returns></returns>
		public static eMeetingType GetMeetingType(eDialProtocol protocol)
		{
			switch (protocol)
			{
				case eDialProtocol.Zoom:
				case eDialProtocol.ZoomContact:
				case eDialProtocol.Sip:
					return eMeetingType.VideoConference;

				case eDialProtocol.Pstn:
					return eMeetingType.AudioConference;

				case eDialProtocol.Unknown:
					return eMeetingType.Presentation;

				default:
					throw new ArgumentOutOfRangeException("protocol");
			}
		}
	}
}
