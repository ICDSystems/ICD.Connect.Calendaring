using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Components.Bookings
{
	public class UsersComponent : AbstractRobinServiceDeviceComponent
    {
		public event EventHandler OnUsersUpdated;

		private readonly Dictionary<string, User> m_Users;
        private readonly RobinServiceDevice m_RobinServiceDevice;

		public UsersComponent(RobinServiceDevice robinServiceDevice)
			: base(robinServiceDevice)
		{
			m_Users = new Dictionary<string, User>();
		    m_RobinServiceDevice = robinServiceDevice;
		}

		protected override void DisposeFinal()
		{
			OnUsersUpdated = null;

			base.DisposeFinal();
		}

		#region Methods

		/// <summary>
		/// Get the User for this resource
		/// </summary>
		public User GetUser(string userId)
		{
			User user;
			bool hasUser = m_Users.TryGetValue(userId, out user);

			if(hasUser)
				return user;

			user = GetUserInfo(userId);

			if (user != null)
				m_Users.Add(userId, user);

			return user;
		}

		#endregion

		#region Private Methods

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

		#endregion
    }
}