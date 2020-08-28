using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICD.Common.Utils;
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
				DateTime utcNow = IcdEnvironment.GetUtcTime();

				Event[] events = GetReservations(Parent.ResourceId, utcNow.StartOfDay(), utcNow.EndOfDay());
				if (events.Length == 0 && m_Events.Count == 0)
					return;

				foreach (var @event in events)
				{
					User user = null;
					if (@event.OrganizerId != null)
						user = m_UsersComponent.GetUser(@event.OrganizerId);
					@event.OrganizerName = user == null ? null : user.UserName;
				}

				m_Events.Clear();
				m_Events.AddRange(events);
			}
			catch (Exception e)
			{
				Parent.Logger.Log(eSeverity.Error, "Failed to get reservations - {0}", e.Message);
			}

			OnEventsUpdated.Raise(this);
		}

		/// <summary>
		/// Creates a booking on the room.
		/// </summary>
		public void CreateEvent(Event booking)
		{
			string data = JsonConvert.SerializeObject(booking, Formatting.None, RobinJsonSerializerSettings);
			PostEvent(Parent.ResourceId, data);
			UpdateBookings();
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
			string path =
				string.Format("spaces/{0}/events?after={1:yyyy-MM-ddTHH:mm:sszzz}&before={2:yyyy-MM-ddTHH:mm:sszzz}&per_page=300",
				              Uri.EscapeDataString(resourceId), startTime, endTime);

			string data = Parent.GetRequest(path);

			return JsonConvert.DeserializeObject<Event[]>(data);
		}

		private void PostEvent(string resourceId, string eventData)
		{
			string path = string.Format("spaces/{0}/events", Uri.EscapeDataString(resourceId));
			Parent.PostRequest(path, Encoding.UTF8.GetBytes(eventData));
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
