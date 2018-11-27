using System;
using System.Collections.Generic;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Components.Users
{
	public sealed class UsersComponent : AbstractRobinServiceDeviceComponent
    {
		public event EventHandler OnUsersUpdated;

		private readonly Dictionary<string, User> m_Users;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="robinServiceDevice"></param>
		public UsersComponent(RobinServiceDevice robinServiceDevice)
			: base(robinServiceDevice)
		{
			m_Users = new Dictionary<string, User>();
		}

		/// <summary>
		/// Release resources.
		/// </summary>
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
			if (m_Users.TryGetValue(userId, out user))
				return user;

			try
			{
				user = GetUserInfo(userId);
			}
			catch (Exception e)
			{
				Parent.Log(eSeverity.Error, "Failed to get user - {0}", e.Message);
			}

			m_Users.Add(userId, user);

			OnUsersUpdated.Raise(this);

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
