using ICD.Common.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ICD.Common.Utils.Xml;
using ICD.Connect.Conferencing.DialContexts;

namespace ICD.Connect.Calendaring.CalendarParsers.Parsers
{
	/// <summary>
	/// RegexPlanMatcher uses regex to match numbers.
	/// </summary>
	public sealed class RegexCalendarParser : AbstractCalendarParser
	{
		public string Pattern { get; set; }

		public string GroupName { get; set; }

		public string SubstitutionPattern { get; set; }

		public string SubstitutionReplacement { get; set; }

		public eDialProtocol Protocol { get; set; }

		/// <summary>
		/// Parses text into BookingProtocolInfo collection
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public override IEnumerable<BookingProtocolInfo> ParseText(string text)
		{
			if (string.IsNullOrEmpty(text))
				yield break;

			var matchCollection = Regex.Matches(text, Pattern);

			foreach (Match match in matchCollection)
			{
				string meetingNumber = string.IsNullOrEmpty(GroupName) ? match.Groups[0].Value : match.Groups[GroupName].Value;

				meetingNumber = string.IsNullOrEmpty(SubstitutionPattern)
					? meetingNumber
					: Regex.Replace(meetingNumber, SubstitutionPattern, SubstitutionReplacement);

				yield return new BookingProtocolInfo
				{
					DialProtocol = Protocol,
					Number = meetingNumber
				};
			}
		}

		public static RegexCalendarParser FromXml(string xml)
		{
			return new RegexCalendarParser
			{
				Pattern = XmlUtils.TryReadChildElementContentAsString(xml, "Pattern"),
				GroupName = XmlUtils.TryReadChildElementContentAsString(xml, "Group"),
				SubstitutionPattern = XmlUtils.TryReadChildElementContentAsString(xml, "ReplacePattern"),
				SubstitutionReplacement = XmlUtils.TryReadChildElementContentAsString(xml, "ReplaceReplacement"),
				Protocol = XmlUtils.TryReadChildElementContentAsEnum<eDialProtocol>(xml, "Protocol", true) ?? eDialProtocol.None
			};
		}
	}
}
