using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Calendaring.Robin.Components.Users;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Components.Events
{
	public sealed class EventsComponent : AbstractRobinServiceDeviceComponent
    {
		public event EventHandler OnEventsUpdated;

		private readonly List<Event> m_Events;
	    private readonly UsersComponent m_UsersComponent;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="robinServiceDevice"></param>
		public EventsComponent(RobinServiceDevice robinServiceDevice)
			: base(robinServiceDevice)
		{
			m_Events = new List<Event>();
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
	        try
	        {
				Event[] events = GetReservations(Parent.ResourceId, DateTime.Today, DateTime.Today.AddDays(1));
				foreach (var @event in events)
				{
					if (@event.OrganizerId != null)
						@event.OrganizerName = m_UsersComponent.GetUser(@event.OrganizerId).UserName;
				}

				m_Events.Clear();
				m_Events.AddRange(events);
	        }
	        catch (Exception e)
	        {
		        Parent.Log(eSeverity.Error, "Failed to get reservations - {0}", e.Message);
	        }

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
			string path = string.Format("spaces/{0}/events?after={1:yyyy-MM-ddTHH:mm:sszzz}&before={2:yyyy-MM-ddTHH:mm:sszzz}&per_page=300", resourceId, startTime, endTime);

			string data = Parent.Request(path);

			return JsonConvert.DeserializeObject<Event[]>(data);
		}

		/// <summary>
		/// Override to get initial values from the service.
		/// </summary>
		protected override void Initialize()
		{
			UpdateBookings();
		}

		#endregion
	}
}