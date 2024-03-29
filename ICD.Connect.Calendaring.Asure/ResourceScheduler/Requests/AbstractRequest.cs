﻿using System;
#if SIMPLSHARP
using Crestron.SimplSharp.Net;
#else
using System.Web;
#endif
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Xml;
using ICD.Connect.Protocol.Network.Ports.Web;
using ICD.Connect.Calendaring.Asure.ResourceScheduler.Results;

namespace ICD.Connect.Calendaring.Asure.ResourceScheduler.Requests
{
	/// <summary>
	/// Base class for ResourceSchedulerService requests.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class AbstractRequest<T>
		where T : AbstractResult
	{
		private const string XLMNS_NS = "http://PeopleCube.ResourceScheduler.WebService/2007/05";

		private const string TEMPLATE =
			@"<x:Envelope xmlns:x=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""{0}"">{1}{2}</x:Envelope>";

		private const string HEADER_TEMPLATE = @"<x:Header>
        <ns:RSCredentials>
            {0}
			{1}
        </ns:RSCredentials>
    </x:Header>";

		private const string BODY_TEMPLATE = @"<x:Body>
        <ns:{0}>
            <ns:request>
                {1}
            </ns:request>
        </ns:{2}>
    </x:Body>";

		private const string PARAMETER_TEMPLATE = @"<ns:{0}>{1}</ns:{2}>";
		private const string USERNAME_TEMPLATE = @"<ns:Username>{0}</ns:Username>";
		private const string PASSWORD_TEMPLATE = @"<ns:Password>{0}</ns:Password>";

		/// <summary>
		/// Gets the name of the service method.
		/// </summary>
		protected abstract string SoapAction { get; }

		/// <summary>
		/// Dispatches the request and parses the response.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Web port failed to dispatch</exception>
		public T Dispatch(IWebPort port)
		{
			if (port == null)
				throw new ArgumentNullException("port");

			if (port.Uri == null)
			{
				string message = string.Format("{0} failed to dispatch - Port URI is NULL", GetType().Name);
				throw new InvalidOperationException(message);
			}

			string action = string.Format("{0}/{1}", XLMNS_NS, SoapAction);
			string bodyXml = GetBody();
			string headerXml = GetHeader(HttpUtility.UrlDecode(port.Uri.GetUserName()),
			                             HttpUtility.UrlDecode(port.Uri.GetPassword()));

			string content = string.Format(TEMPLATE, XLMNS_NS, headerXml, bodyXml);

			WebPortResponse output;

			try
			{
				output = port.DispatchSoap(action, content);
			}
			// Catch HTTP or HTTPS exception, without dependency on Crestron
			catch (Exception e)
			{
				string message = string.Format("{0} failed to dispatch - {1}", GetType().Name, e.Message);
				throw new InvalidOperationException(message, e);
			}

			if (!output.GotResponse)
			{
				string message = string.Format("{0} failed to dispatch", GetType().Name);
				throw new InvalidOperationException(message);
			}

			if (string.IsNullOrEmpty(output.DataAsString))
			{
				string message = string.Format("{0} failed to dispatch - received empty response", GetType().Name);
				throw new InvalidOperationException(message);
			}

			return ParseResponse(output.DataAsString);
		}

		#region Private Methods

		/// <summary>
		/// Parses the response data from a request.
		/// </summary>
		/// <param name="content"></param>
		private T ParseResponse(string content)
		{
			try
			{
				content = StripSoapXml(content);
				return ResultFromXml(content);
			}
			catch (Exception e)
			{
				string message = string.Format("Failed to parse content: {0}", content);
				throw new FormatException(message, e);
			}
		}

		/// <summary>
		/// Builds the resulting object from the xml response.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		protected abstract T ResultFromXml(string xml);

		/// <summary>
		/// Gets the header xml for the request.
		/// </summary>
		/// <returns></returns>
		private string GetHeader(string username, string password)
		{
			string usernameXml = string.IsNullOrEmpty(username)
				                     ? null
				                     : string.Format(USERNAME_TEMPLATE, username);

			string passwordXml = string.IsNullOrEmpty(password)
				                     ? null
				                     : string.Format(PASSWORD_TEMPLATE, password);

			return string.Format(HEADER_TEMPLATE, usernameXml, passwordXml);
		}

		/// <summary>
		/// Gets the body xml for the request.
		/// </summary>
		/// <returns></returns>
		protected virtual string GetBody()
		{
			string parameters = BuildParametersXml();
			return string.Format(BODY_TEMPLATE, SoapAction, parameters, SoapAction);
		}

		/// <summary>
		/// Builds the parameter elements for the body xml.
		/// </summary>
		/// <returns></returns>
		private string BuildParametersXml()
		{
			string output = string.Empty;
			Action<string, object> callback = (s, o) => output += string.Format(PARAMETER_TEMPLATE, s, o, s);
			AddSoapParams(callback);
			return output;
		}

		/// <summary>
		/// Adds the parameters by calling addParam for each name/value pair.
		/// </summary>
		/// <param name="addParam"></param>
		protected virtual void AddSoapParams(Action<string, object> addParam)
		{
		}

		/// <summary>
		/// Returns the result xml without the surrounding soap elements.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		private static string StripSoapXml(string xml)
		{
			string soapBody = XmlUtils.GetInnerXml(xml);
			string response = XmlUtils.GetInnerXml(soapBody);
			return XmlUtils.GetInnerXml(response);
		}

		#endregion
	}
}
