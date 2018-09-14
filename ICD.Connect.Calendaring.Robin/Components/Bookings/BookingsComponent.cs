using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Calendaring.Robin.Service;

namespace ICD.Connect.Calendaring.Robin.Components.Bookings
{
	public class BookingsComponent : AbstractRobinServiceDeviceComponent
    {
		public event EventHandler OnBookingsUpdated;

		private readonly List<Booking> m_Bookings;
        private readonly RobinServiceDevice m_RobinServiceDevice;

		public BookingsComponent(RobinServiceDevice robinServiceDevice)
			: base(robinServiceDevice)
		{
			m_Bookings = new List<Booking>();
		    m_RobinServiceDevice = robinServiceDevice;
		}

		protected override void DisposeFinal()
		{
			OnBookingsUpdated = null;

			base.DisposeFinal();
		}

		#region Methods

		/// <summary>
		/// Get the cached bookings for this resource
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Booking> GetBookings()
		{
			return m_Bookings.ToList();
		}

        /// <summary>
        /// Updates the bookings on the room.
        /// </summary>
        /// <remarks>
        /// If tied to a Google Calendar, call this method no more than every 10 minutes,
        /// as many rooms polling the calendar can hit the query limit quickly and either
        /// cut off further queries or start charging the customer for queries.
        /// </remarks>
        public void UpdateBookings()
        {
            m_Bookings.Clear();
            var bookings = ResourceSchedulerService.GetReservations(m_RobinServiceDevice.GetPort(), m_RobinServiceDevice.ResourceId, DateTime.Today, DateTime.Today.AddDays(1));
            foreach (var booking in bookings)
            {
                if (booking.OrganizerId != null)
                {
                    booking.OrganizerName = ResourceSchedulerService.GetUserInfo(m_RobinServiceDevice.GetPort(), booking.OrganizerId).UserName;
                }
            }

            m_Bookings.AddRange(bookings);

            OnBookingsUpdated.Raise(this);
        }

        protected override void Initialize()
		{
			base.Initialize();
            UpdateBookings();

		    OnBookingsUpdated.Raise(this);
		}

		#endregion

	}
}