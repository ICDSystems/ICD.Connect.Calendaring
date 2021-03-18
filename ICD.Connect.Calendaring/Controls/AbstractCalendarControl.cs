using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Calendaring.Bookings;
using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Calendaring.Controls
{
    public abstract class AbstractCalendarControl<T> : AbstractDeviceControl<T>, ICalendarControl
	    where T : IDevice
    {
	    #region Events

	    /// <summary>
	    /// Raised when bookings are added/removed.
	    /// </summary>
	    public abstract event EventHandler OnBookingsChanged;

	    public event EventHandler<GenericEventArgs<eCalendarFeatures>> OnSupportedCalendarFeaturesChanged;

	    #endregion

	    private eCalendarFeatures m_SupportedCalendarFeatures;

	    public eCalendarFeatures SupportedCalendarFeatures
	    {
		    get { return m_SupportedCalendarFeatures; }
		    protected set
		    {
				if (value == m_SupportedCalendarFeatures)
					return;

				m_SupportedCalendarFeatures = value;

				OnSupportedCalendarFeaturesChanged.Raise(this, value);
		    }
	    }

	    #region Constructor

	    /// <summary>
	    /// Constructor.
	    /// </summary>
	    /// <param name="parent"></param>
	    /// <param name="id"></param>
	    protected AbstractCalendarControl(T parent, int id)
		    : base(parent, id)
	    {
	    }

	    #endregion

	    #region Methods

	    /// <summary>
	    /// Updates the view.
	    /// </summary>
	    [PublicAPI]
	    public abstract void Refresh();

	    /// <summary>
		/// Gets the collection of calendar bookings.
	    /// </summary>
	    [PublicAPI]
	    public abstract IEnumerable<IBooking> GetBookings();

	    /// <summary>
	    /// Pushes the booking to the calendar service.
	    /// </summary>
	    /// <param name="booking"></param>
	    public abstract void PushBooking(IBooking booking);

	    /// <summary>
	    /// Edits the selected booking with the calendar service.
	    /// </summary>
	    /// <param name="oldBooking"></param>
	    /// <param name="newBooking"></param>
	    public abstract void EditBooking(IBooking oldBooking, IBooking newBooking);

	    /// <summary>
	    /// Returns true if the booking argument can be checked in.
	    /// </summary>
	    /// <returns></returns>
	    public abstract bool CanCheckIn(IBooking booking);

	    /// <summary>
	    /// Returns true if the booking argument can be checked out of.
	    /// </summary>
	    /// <param name="booking"></param>
	    /// <returns></returns>
	    public abstract bool CanCheckOut(IBooking booking);

	    /// <summary>
	    /// Checks in to the specified booking.
	    /// </summary>
	    /// <param name="booking"></param>
	    public abstract void CheckIn(IBooking booking);

	    /// <summary>
	    /// Checks out of the specified booking.
	    /// </summary>
	    /// <param name="booking"></param>
	    public abstract void CheckOut(IBooking booking);

	    #endregion

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
