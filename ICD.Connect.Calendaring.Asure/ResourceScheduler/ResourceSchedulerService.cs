using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Connect.Protocol.Network.Ports.Web;
using ICD.Connect.Calendaring.Asure.ResourceScheduler.Requests;
using ICD.Connect.Calendaring.Asure.ResourceScheduler.Results;

namespace ICD.Connect.Calendaring.Asure.ResourceScheduler
{
	public static class ResourceSchedulerService
	{
		#region Methods

		/// <summary>
		/// Checks in to the given reservation.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="reservationId"></param>
		/// <returns></returns>
		[PublicAPI]
		public static CheckInResult CheckIn(IWebPort port, int reservationId)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new CheckInRequest(reservationId).Dispatch(port);
		}

		[PublicAPI]
		public static CheckInNowResult CheckInNow(IWebPort port, int reservationId)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new CheckInNowRequest(reservationId).Dispatch(port);
		}

		/// <summary>
		/// Checks out of the given reservation.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="reservationId"></param>
		/// <returns></returns>
		[PublicAPI]
		public static CheckOutResult CheckOut(IWebPort port, int reservationId)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new CheckOutRequest(reservationId).Dispatch(port);
		}

		/// <summary>
		/// Deletes the reservation with the given id.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="reservationId"></param>
		/// <returns></returns>
		[PublicAPI]
		public static DeleteReservationResult DeleteReservation(IWebPort port, int reservationId)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new DeleteReservationRequest(reservationId).Dispatch(port);
		}

		/// <summary>
		/// Gets the regions.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		[PublicAPI]
		public static GetRegionsResult GetRegions(IWebPort port)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new GetRegionsRequest().Dispatch(port);
		}

		/// <summary>
		/// Gets the locations.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		[PublicAPI]
		public static GetLocationsResult GetLocations(IWebPort port)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new GetLocationsRequest().Dispatch(port);
		}

		/// <summary>
		/// Gets the locations for the given region.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="regionId"></param>
		/// <returns></returns>
		[PublicAPI]
		public static GetLocationsResult GetLocations(IWebPort port, int regionId)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new GetLocationsRequest(regionId).Dispatch(port);
		}

		/// <summary>
		/// Gets all regions and their child locations.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		[PublicAPI]
		public static GetAllRegionsAndLocationsResult GetAllRegionsAndLocations(IWebPort port)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new GetAllRegionsAndLocationsRequest().Dispatch(port);
		}

		/// <summary>
		/// Gets the reservation with the given id.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[PublicAPI]
		public static GetReservationResult GetReservation(IWebPort port, int id)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new GetReservationRequest(id).Dispatch(port);
		}

		/// <summary>
		/// Gets all of the reservations between the start and end dates.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		[PublicAPI]
		public static GetReservationsResult GetReservations(IWebPort port, DateTime start, DateTime end)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new GetReservationsRequest(start, end).Dispatch(port);
		}

		/// <summary>
		/// Gets all of the reservations between the start and end date with the given attendee.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="attendeeId"></param>
		/// <returns></returns>
		[PublicAPI]
		public static GetReservationsByAttendeeResult GetReservationsByAttendee(IWebPort port, DateTime start,
		                                                                        DateTime end, int attendeeId)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new GetReservationsByAttendeeRequest(start, end, attendeeId).Dispatch(port);
		}

		/// <summary>
		/// Gets all of the reservations between the start and end date with the given location.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="locationId"></param>
		/// <returns></returns>
		[PublicAPI]
		public static GetReservationsByLocationResult GetReservationsByLocation(IWebPort port, DateTime start,
		                                                                        DateTime end, int locationId)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new GetReservationsByLocationRequest(start, end, locationId).Dispatch(port);
		}

		/// <summary>
		/// Gets all of the reservations between the start and end date with the given resource.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="resourceId"></param>
		/// <returns></returns>
		[PublicAPI]
		public static GetReservationsByResourceResult GetReservationsByResource(IWebPort port, DateTime start,
		                                                                        DateTime end, int resourceId)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			return new GetReservationsByResourceRequest(start, end, resourceId).Dispatch(port);
		}

		/// <summary>
		/// Creates a new reservation.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="description"></param>
		/// <param name="notes"></param>
		/// <param name="resourceIds"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		[PublicAPI]
		public static SubmitReservationResult SubmitReservation(IWebPort port,
		                                                        string description, string notes,
		                                                        IEnumerable<int> resourceIds,
		                                                        DateTime start, DateTime end)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			SubmitReservationRequest request = new SubmitReservationRequest(description, notes, resourceIds, start, end);
			return request.Dispatch(port);
		}

		#endregion
	}
}
