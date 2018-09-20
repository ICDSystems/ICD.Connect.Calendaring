using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if SIMPLSHARP
using Crestron.SimplSharp;
#endif
namespace ICD.Connect.Calendaring.Booking
{
	public interface IPstnBooking : IBooking
	{
		string PhoneNumber { get; }
	}
}