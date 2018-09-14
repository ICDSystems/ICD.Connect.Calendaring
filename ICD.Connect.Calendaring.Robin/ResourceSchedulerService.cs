using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Connect.Protocol.Network.WebPorts;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Robin.Service
{
	public static class ResourceSchedulerService
	{

        #region Methods

        /// <summary>
        /// Gets all of the reservations between the start and end date with the given resource.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="resourceId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [PublicAPI]
		public static Components.Bookings.Booking[] GetReservations(IWebPort port, string resourceId, DateTime startTime, DateTime endTime)
		{
			if (port == null)
				throw new ArgumentNullException("port");

		    string uri = string.Format("spaces/{0}/events?after={1:yyyy-MM-ddTHH:mm:ssZ}&before={2:yyyy-MM-ddTHH:mm:ssZ}", resourceId, startTime, endTime);

		    Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>();
		    headers.Add("Authorization", new List<string>() { "Access-Token q5NHYiVud5r7CHCu1SV0Nrr6mTNIMvdCLPccnmreQPXZjMQ4Q6o5EkHtpNPYObfBlv20v7AOuuRHgdLfvsmvSAyuFLAWPitzRXxjdAWY3i5fi3QyA2TSZfQFHPOhBHuw" });
		    headers.Add("Connection", new List<string>() { "keep-alive" });

		    bool success = false;
            string response = "";
		    
		    try
		    {
		        success = port.Get(uri, headers, out response);
            }
		    // Catch HTTP or HTTPS exception, without dependency on Crestron
		    catch (Exception e)
		    {
		        string message = string.Format("{0} failed to dispatch - {1}", "GetReservations", e.Message);
		        throw new InvalidOperationException(message, e);
		    }

		    if (!success)
		    {
		        string message = string.Format("{0} failed to dispatch", "GetReservations");
		        throw new InvalidOperationException(message);
		    }

		    if (string.IsNullOrEmpty(response))
		    {
		        string message = string.Format("{0} failed to dispatch - received empty response", "GetReservations");
		        throw new InvalidOperationException(message);
		    }

		    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response)["data"];

            Components.Bookings.Booking[] bookings = JsonConvert.DeserializeObject<Components.Bookings.Booking[]>(data.ToString());

		    return bookings;
		}

        /// <summary>
        /// Gets all of the reservations between the start and end date with the given resource.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [PublicAPI]
        public static Components.Bookings.UserInfo GetUserInfo(IWebPort port, string userId)
        {
            if (port == null)
                throw new ArgumentNullException("port");

            string uri = string.Format("users/{0}", userId);

            Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>();
            headers.Add("Authorization", new List<string>() { "Access-Token q5NHYiVud5r7CHCu1SV0Nrr6mTNIMvdCLPccnmreQPXZjMQ4Q6o5EkHtpNPYObfBlv20v7AOuuRHgdLfvsmvSAyuFLAWPitzRXxjdAWY3i5fi3QyA2TSZfQFHPOhBHuw" });
            headers.Add("Connection", new List<string>() { "keep-alive" });

            bool success = false;
            string response = "";

            try
            {
                success = port.Get(uri, headers, out response);
            }
            // Catch HTTP or HTTPS exception, without dependency on Crestron
            catch (Exception e)
            {
                string message = string.Format("{0} failed to dispatch - {1}", "GetReservations", e.Message);
                throw new InvalidOperationException(message, e);
            }

            if (!success)
            {
                string message = string.Format("{0} failed to dispatch", "GetReservations");
                throw new InvalidOperationException(message);
            }

            if (string.IsNullOrEmpty(response))
            {
                string message = string.Format("{0} failed to dispatch - received empty response", "GetReservations");
                throw new InvalidOperationException(message);
            }

            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response)["data"];

            Components.Bookings.UserInfo user = JsonConvert.DeserializeObject<Components.Bookings.UserInfo>(data.ToString());

            return user;
        }

        #endregion
    }
}
