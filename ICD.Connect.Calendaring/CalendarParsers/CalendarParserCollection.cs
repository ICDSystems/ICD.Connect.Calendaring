using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Xml;
using ICD.Connect.Calendaring.CalendarParsers.Parsers;

namespace ICD.Connect.Calendaring.CalendarParsers
{
	/// <summary>
	/// XmlCalendarParser loads a configuration from XML.
	/// </summary>
	public sealed class CalendarParserCollection
	{
		private readonly Dictionary<ICalendarParser, int> m_Parsers;
		private readonly SafeCriticalSection m_ParsersSection;

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public CalendarParserCollection()
		{
			m_Parsers = new Dictionary<ICalendarParser, int>();
			m_ParsersSection = new SafeCriticalSection();
		}

		public IEnumerable<ICalendarParser> Parsers
		{
			get { return m_Parsers.Keys.ToArray(m_Parsers.Count); }
		}

		#endregion

		#region Matchers

		/// <summary>
		/// Clears the collection of parsers.
		/// </summary>
		public void ClearMatchers()
		{
			m_ParsersSection.Execute(() => m_Parsers.Clear());
		}

		/// <summary>
		/// Parses the xml to build the collection of parsers.
		/// </summary>
		/// <param name="xml"></param>
		public void LoadParsers(string xml)
		{
			m_ParsersSection.Enter();

			try
			{
				ClearMatchers();

				string parsersXml = XmlUtils.GetChildElementAsString(xml, "CalendarParsers");

				foreach (IcdXmlReader child in XmlUtils.GetChildElements(parsersXml))
				{
					ParseParsers(child);
					child.Dispose();
				}
			}
			finally
			{
				m_ParsersSection.Leave();
			}
		}

		#endregion

		#region Methods

		#endregion

		#region Private Methods


		/// <summary>
		/// Parses the xml for a single parsers.
		/// </summary>
		/// <param name="reader"></param>
		private void ParseParsers(IcdXmlReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			foreach (IcdXmlReader child in reader.GetChildElements())
			{
				ICalendarParser parser;
				int order;

				switch (child.Name)
				{
					case "RegexParser":
						parser = new RegexCalendarParser(child.GetAttributeAsString("Pattern"),
							child.GetAttributeAsString("Group"),
							child.GetAttributeAsString("ReplacePattern"),
							child.GetAttributeAsString("ReplaceReplacement"));
						order = 0;
						break;

					default:
						throw new ArgumentOutOfRangeException("Unknown element: " + child.Name);
				}

				m_Parsers[parser] = order;

				child.Dispose();
			}
		}
		#endregion
	}
}
