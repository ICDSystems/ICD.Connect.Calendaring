using System;
using System.Collections.Generic;
using System.Text;

namespace ICD.Connect.Calendaring.Booking
{
    public abstract class AbstractBookingNumber : IBookingNumber
    {
	    public abstract eBookingProtocol Protocol { get; }
    }
}
