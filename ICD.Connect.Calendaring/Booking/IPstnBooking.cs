using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ICD.Connect.Calendaring.Booking
{
	public interface IPstnBooking : IBooking
	{
		string PhoneNumber { get; }
	}
}