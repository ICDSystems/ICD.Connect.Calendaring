using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Conferencing.Zoom.Controls.Calendar;
using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Calendaring_NetStandard.CalendarControl
{
    public abstract class AbstractCalendarControl<T> : AbstractDeviceControl<T>, ICalendarControl
	    where T : IDeviceBase
    {
	    /// <summary>
	    /// Raised when bookings are added/removed.
	    /// </summary>
	    public abstract event EventHandler OnBookingsChanged;

	    /// <summary>
	    /// Updates the view.
	    /// </summary>
	    [PublicAPI]
	    public abstract void Refresh();

	    /// <summary>
	    /// Updates the view.
	    /// </summary>
	    [PublicAPI]
	    public abstract IEnumerable<IBooking> GetBookings();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
	    protected AbstractCalendarControl(T parent, int id)
		    : base(parent, id)
	    {
	    }

	    #region Console

	    /// <summary>
	    /// Gets the child console commands.
	    /// </summary>
	    /// <returns></returns>
	    public override IEnumerable<IConsoleCommand> GetConsoleCommands()
	    {
		    foreach (IConsoleCommand command in GetBaseConsoleCommands())
			    yield return command;

		    foreach (IConsoleCommand command in CalendarControlConsole.GetConsoleCommands(this))
			    yield return command;
	    }

	    /// <summary>
	    /// Workaround for "unverifiable code" warning.
	    /// </summary>
	    /// <returns></returns>
	    private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
	    {
		    return base.GetConsoleCommands();
	    }

	    /// <summary>
	    /// Calls the delegate for each console status item.
	    /// </summary>
	    /// <param name="addRow"></param>
	    public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
	    {
		    base.BuildConsoleStatus(addRow);

		    CalendarControlConsole.BuildConsoleStatus(this, addRow);
	    }

	    /// <summary>
	    /// Gets the child console nodes.
	    /// </summary>
	    /// <returns></returns>
	    public override IEnumerable<IConsoleNodeBase> GetConsoleNodes()
	    {
		    foreach (IConsoleNodeBase node in GetBaseConsoleNodes())
			    yield return node;

		    foreach (IConsoleNodeBase node in CalendarControlConsole.GetConsoleNodes(this))
			    yield return node;
	    }

	    /// <summary>
	    /// Workaround for "unverifiable code" warning.
	    /// </summary>
	    /// <returns></returns>
	    private IEnumerable<IConsoleNodeBase> GetBaseConsoleNodes()
	    {
		    return base.GetConsoleNodes();
	    }

	    #endregion
	}
}
