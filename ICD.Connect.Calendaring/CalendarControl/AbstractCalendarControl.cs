using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;

namespace ICD.Connect.Calendaring_NetStandard
{
    public abstract class AbstractCalendarControl : ICalendarControl
	{
		/// <summary>
	    /// Updates the view.
	    /// </summary>
	    [PublicAPI]
	    public virtual void Refresh()
	    {
	    }

		/// <summary>
		/// Updates the view.
		/// </summary>
		[PublicAPI]
		public virtual IEnumerable<IBooking> GetBookings()
		{
			throw new NotImplementedException();
		}
	}
}
