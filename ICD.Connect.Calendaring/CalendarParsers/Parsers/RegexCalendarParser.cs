using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ICD.Common.Properties;
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

		public string PasswordGroup { get; set; }

		public string SubstitutionPattern { get; set; }

		public string SubstitutionReplacement { get; set; }

		public eDialProtocol Protocol { get; set; }

		/// <summary>
		/// Parses text into BookingProtocolInfo collection
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		[CanBeNull]
		public override IDialContext ParseLine(string text)
		{
			if (string.IsNullOrEmpty(text))
				return null;

			Match match = Regex.Match(text, Pattern);

			if (!match.Success)
				return null;
			
			string meetingNumber = string.IsNullOrEmpty(GroupName)
					                       ? match.Value
					                       : match.Groups[GroupName].Value;

			string meetingPassword = string.IsNullOrEmpty(PasswordGroup)
					                         ? null
					                         : match.Groups[PasswordGroup].Value;

			meetingNumber = string.IsNullOrEmpty(SubstitutionPattern)
					                ? meetingNumber
					                : Regex.Replace(meetingNumber, SubstitutionPattern, SubstitutionReplacement);

			return new DialContext
			{
				Protocol = Protocol,
				DialString = meetingNumber,
				Password = meetingPassword
			};
			
		}

		public static RegexCalendarParser FromXml(string xml)
		{
			return new RegexCalendarParser
			{
				Pattern = XmlUtils.TryReadChildElementContentAsString(xml, "Pattern"),
				GroupName = XmlUtils.TryReadChildElementContentAsString(xml, "Group"),
				PasswordGroup = XmlUtils.TryReadChildElementContentAsString(xml, "PasswordGroup"),
				SubstitutionPattern = XmlUtils.TryReadChildElementContentAsString(xml, "ReplacePattern"),
				SubstitutionReplacement = XmlUtils.TryReadChildElementContentAsString(xml, "ReplaceReplacement"),
				Protocol = XmlUtils.TryReadChildElementContentAsEnum<eDialProtocol>(xml, "Protocol", true) ?? eDialProtocol.Unknown
			};
		}
	}
}
