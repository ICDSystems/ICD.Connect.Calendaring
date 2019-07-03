using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if SIMPLSHARP
using Crestron.SimplSharp.CrestronIO;
#else
using System.IO;
#endif
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.IO;
using ICD.Common.Utils.Xml;
using ICD.Connect.Calendaring.CalendarParsers.Parsers;

namespace ICD.Connect.Calendaring.CalendarParsers
{
	/// <summary>
	/// XmlCalendarParser loads a configuration from XML.
	/// </summary>
	public sealed class CalendarParserCollection
	{
		private readonly List<ICalendarParser> m_Parsers;
		private readonly SafeCriticalSection m_ParsersSection;

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public CalendarParserCollection()
		{
			m_Parsers = new List<ICalendarParser>();
			m_ParsersSection = new SafeCriticalSection();
		}

		public IEnumerable<ICalendarParser> GetParsers()
		{
			return m_Parsers.ToArray(m_Parsers.Count);
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
		/// <param name="path"></param>
		public void LoadParsers(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("Path can not be null or empty");

			m_ParsersSection.Enter();

			string calendarParserPath = PathUtils.GetDefaultConfigPath("CalendarParsing", path);
			if (!IcdFile.Exists(calendarParserPath))
				throw new FileNotFoundException("No file at path " + calendarParserPath);

			string xml = IcdFile.ReadToEnd(calendarParserPath, new UTF8Encoding(false));
			xml = EncodingUtils.StripUtf8Bom(xml);

			try
			{
				ClearMatchers();

				foreach (string child in XmlUtils.GetChildElementsAsString(xml))
					DeserializeParser(child);
			}
			finally
			{
				m_ParsersSection.Leave();
			}
		}

		#endregion

		#region Methods

		public IEnumerable<BookingProtocolInfo> ParseText(string text)
		{
			return GetParsers().SelectMany(p => p.ParseText(text)).Distinct();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Parses the xml for a single parsers.
		/// </summary>
		/// <param name="xml"></param>
		private void DeserializeParser(string xml)
		{
			if (string.IsNullOrEmpty(xml))
				throw new ArgumentNullException("xml");

			ICalendarParser parser;

			switch (XmlUtils.ReadElementName(xml))
			{
				case "RegexParser":
					parser = RegexCalendarParser.FromXml(xml);
					break;

				default:
					return;
			}

			m_Parsers.Add(parser);
		}

		#endregion
	}
}
