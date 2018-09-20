using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Components.Bookings
{
	public class EventsComponent : AbstractRobinServiceDeviceComponent
    {
		public event EventHandler OnEventsUpdated;

		private readonly List<Event> m_Events;
        private readonly RobinServiceDevice m_RobinServiceDevice;
	    private readonly UsersComponent m_UsersComponent;

		public EventsComponent(RobinServiceDevice robinServiceDevice)
			: base(robinServiceDevice)
		{
			m_Events = new List<Event>();
		    m_RobinServiceDevice = robinServiceDevice;
			m_UsersComponent = robinServiceDevice.Components.GetComponent<UsersComponent>();
		}

		protected override void DisposeFinal()
		{
			OnEventsUpdated = null;

			base.DisposeFinal();
		}

		#region Methods

		/// <summary>
		/// Get the cached bookings for this resource
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Event> GetEvents()
		{
			return m_Events.ToList();
		}

        /// <summary>
        /// Updates the bookings on the room.
        /// </summary>
        public void UpdateBookings()
        {
            m_Events.Clear();

            Event[] events = GetReservations(m_RobinServiceDevice.ResourceId, DateTime.Today, DateTime.Today.AddDays(1));
            foreach (var @event in events)
            {
                if (@event.OrganizerId != null)
                {
                    @event.OrganizerName = m_UsersComponent.GetUser(@event.OrganizerId).UserName;
                }
            }

            m_Events.AddRange(events);

            OnEventsUpdated.Raise(this);
        }

		#endregion

		#region Private Methods


		/// <summary>
		/// Gets all of the reservations between the start and end date with the given resource.
		/// </summary>
		/// <param name="resourceId"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>
		private Event[] GetReservations(string resourceId, DateTime startTime, DateTime endTime)
		{
			string uri = string.Format("spaces/{0}/events?after={1:yyyy-MM-ddTHH:mm:ssZ}&before={2:yyyy-MM-ddTHH:mm:ssZ}", resourceId, startTime, endTime);

			string data = Parent.Request(uri);

			return JsonConvert.DeserializeObject<Event[]>(data);
		}

		/// <summary>
		/// Gets all of the reservations between the start and end date with the given resource.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		private User GetUserInfo(string userId)
		{
			string uri = string.Format("users/{0}", userId);

			string data = Parent.Request(uri);

			return JsonConvert.DeserializeObject<User>(data);
		}

		public override void ParentOnOnSetPort(object sender, EventArgs e)
		{
			UpdateBookings();
		}

		#endregion
	}
}