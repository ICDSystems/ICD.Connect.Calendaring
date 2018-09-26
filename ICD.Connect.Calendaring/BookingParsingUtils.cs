using System.Collections.Generic;
using System.Text.RegularExpressions;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.Calendaring
{
	/// <summary>
	/// Util methods for working with the Robin services.
	/// </summary>
	public static class BookingParsingUtils
	{
		private const string ZOOM_REGEX = @"(?'zoomUrl'https:\/\/zoom.us\/j\/(?'meetingNumber'[0-9]+))";

		/// <summary>
		/// Parses meeting data from booking description.
		/// </summary>
		/// <param name="description"></param>
		/// <returns></returns>
		[NotNull]
		public static IEnumerable<BookingProtocolInfo> GetProtocolInfos(string description)
		{
			IEnumerable<string> lines = description.Split("\n");
			foreach (string line in lines)
			{
				Match match;

				if (RegexUtils.Matches(line, ZOOM_REGEX, out match))
				{
					string meetingNumber = match.Groups["meetingNumber"].Value;

					yield return new BookingProtocolInfo
					{
						BookingProtocol = eBookingProtocol.Zoom,
						Number = meetingNumber
					};
				}
			}
		}
	}
}
