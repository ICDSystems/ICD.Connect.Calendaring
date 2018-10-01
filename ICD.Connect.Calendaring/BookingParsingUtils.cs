using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using System.Text.RegularExpressions;

namespace ICD.Connect.Calendaring
{
	/// <summary>
	/// Util methods for working with the Robin services.
	/// </summary>
	public static class BookingParsingUtils
	{
		private static string m_Pattern;
		private static string m_GroupName;
		private static string m_SubstitutionPattern;
		private static string m_SubstitutionReplacement;

		public static string Pattern
		{
			set { m_Pattern = value; }
		}
		public static string GroupName
		{
			set { m_GroupName = value; }
		}
		public static string SubstitutionPattern
		{
			set { m_SubstitutionPattern = value; }
		}
		public static string SubstitutionReplacement
		{
			set { m_SubstitutionReplacement = value; }
		}

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
				var newLine = String.IsNullOrWhiteSpace(m_SubstitutionPattern) || String.IsNullOrWhiteSpace(m_SubstitutionReplacement) ? line : Regex.Replace(line, m_SubstitutionPattern, m_SubstitutionReplacement);
				if (RegexUtils.Matches(newLine, m_Pattern, out match))
				{
					string meetingNumber = match.Groups[m_GroupName].Value;

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
