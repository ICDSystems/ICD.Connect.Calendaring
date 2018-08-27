using System;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Calendaring_NetStandard;

namespace ICD.Connect.Conferencing.Zoom.Controls.Calendar
{
	public static class CalendarControlConsole
	{
		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleNodeBase> GetConsoleNodes(ICalendarControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield break;
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="addRow"></param>
		public static void BuildConsoleStatus(ICalendarControl instance, AddStatusRowDelegate addRow)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleCommand> GetConsoleCommands(ICalendarControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");


			yield return new ConsoleCommand("Refresh", "Refreshes the list of bookings", () => instance.Refresh());
			yield return new ConsoleCommand("PrintBookings", "Prints a table of the available bookings", () => PrintBookings(instance));
		}

		private static string PrintBookings(ICalendarControl instance)
		{
			TableBuilder builder = new TableBuilder("Meeting Name", "Organizer Name", "Organizer Email", "Start Time", "End Time", "Type");


			foreach (IBooking booking in instance.GetBookings())
			{
				builder.AddRow(booking.MeetingName, booking.OrganizerName, booking.OrganizerEmail, booking.StartTime, booking.EndTime, booking.Type);
			}

			return builder.ToString();
		}
	}
}
