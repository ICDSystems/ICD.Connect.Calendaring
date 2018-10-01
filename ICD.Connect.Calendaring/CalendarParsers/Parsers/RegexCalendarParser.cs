using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ICD.Connect.Calendaring.CalendarParsers.Parsers
{
	/// <summary>
	/// RegexPlanMatcher uses regex to match numbers.
	/// </summary>
	public sealed class RegexCalendarParser : AbstractCalendarParser
	{
		private readonly string m_Pattern;
		private readonly string m_GroupName;
		private readonly string m_SubstitutionPattern;
		private readonly string m_SubstitutionReplacement;

		/// <summary>
		/// Constructor.
		/// </summary>
		public RegexCalendarParser(string pattern, string groupName, string substitutionPattern, string substitutionReplacement)
			: base()
		{
			m_Pattern = pattern;
			m_GroupName = groupName;
			m_SubstitutionPattern = substitutionPattern;
			m_SubstitutionReplacement = substitutionReplacement;
		}

		public override IEnumerable<BookingProtocolInfo> ParseText(string text)
		{
			BookingParsingUtils.Pattern = m_Pattern;
			BookingParsingUtils.GroupName = m_GroupName;
			BookingParsingUtils.SubstitutionPattern = m_SubstitutionPattern;
			BookingParsingUtils.SubstitutionReplacement = m_SubstitutionReplacement;
			return BookingParsingUtils.GetProtocolInfos(text);
		}
	}
}
