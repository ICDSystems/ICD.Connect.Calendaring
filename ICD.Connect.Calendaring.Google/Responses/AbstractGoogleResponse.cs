﻿using System;
using ICD.Connect.Calendaring.Google.Converters;
using Newtonsoft.Json;

namespace ICD.Connect.Calendaring.Google.Responses
{
	public abstract class AbstractGoogleResponse : Exception
	{
		public Error Error { get; set; }
	}

	[JsonConverter(typeof(ErrorConverter))]
	public sealed class Error
	{
		public string Code { get; set; }
		public string Message { get; set; }
		public InnerError InnerError { get; set; }
	}

	[JsonConverter(typeof(InnerErrorConverter))]
	public sealed class InnerError
	{
		public string ResquestId { get; set; }
		public string Date { get; set; }
	}
}