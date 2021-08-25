#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using System;
using System.Collections.Generic;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Calendaring.Robin.Components.Converters;
using ICD.Connect.Protocol.Network.Ports.Web;

namespace ICD.Connect.Calendaring.Robin.Components
{
	public abstract class AbstractRobinServiceDeviceComponent : IDisposable, IConsoleNode
	{
		#region Properties

		/// <summary>
		/// Gets the RobinServiceDevice.
		/// </summary>
		public RobinServiceDevice Parent { get; private set; }

		/// <summary>
		/// Gets the name of the node in the console.
		/// </summary>
		public virtual string ConsoleName { get { return GetType().Name; } }

		/// <summary>
		/// Gets the help information for the node.
		/// </summary>
		public virtual string ConsoleHelp { get { return string.Empty; } }

		private static JsonSerializerSettings s_RobinJsonSerializerSettings;

		public static JsonSerializerSettings RobinJsonSerializerSettings
		{
			get
			{
				if (s_RobinJsonSerializerSettings == null)
				{
					s_RobinJsonSerializerSettings = new JsonSerializerSettings
					{
#if !SIMPLSHARP
						// Turn off the ridiculous new behaviour of DateTiming anything vaguely resembling a date
						DateParseHandling = DateParseHandling.None,
#endif
					};

					// Serialize DateTimes to ISO without ms
					s_RobinJsonSerializerSettings.Converters.Add(new DateInfoDateTimeConverter());
				}

				return s_RobinJsonSerializerSettings;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		protected AbstractRobinServiceDeviceComponent(RobinServiceDevice parent)
		{
			Parent = parent;
			Subscribe(Parent);
		}

		/// <summary>
		/// Deconstructor.
		/// </summary>
		~AbstractRobinServiceDeviceComponent()
		{
			Dispose(false);
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		public void Dispose()
		{
            Dispose(true);
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			Unsubscribe(Parent);

			DisposeFinal();
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		protected virtual void DisposeFinal()
		{
		}

        #endregion

        #region Methods

        /// <summary>
        /// Calls the delegate for each console status item.
        /// </summary>
        /// <param name="addRow"></param>
        public virtual void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
		}

		/// <summary>
		/// Gets the child console node groups.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			yield break;
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			yield break;
		}

		/// <summary>
		/// Override to get initial values from the service.
		/// </summary>
		protected virtual void Initialize()
		{
		}

		#endregion

		#region Private

		/// <summary>
		/// Subscribe to the events events.
		/// </summary>
		/// <param name="parent"></param>
		private void Subscribe(RobinServiceDevice parent)
		{
			parent.OnSetPort += ParentOnSetPort;
		}

		/// <summary>
		/// Unsubscribe from the parent port change.
		/// </summary>
		/// <param name="parent"></param>
		private void Unsubscribe(RobinServiceDevice parent)
		{
			parent.OnSetPort -= ParentOnSetPort;
		}

		/// <summary>
		/// Called when the parent port changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ParentOnSetPort(object sender, GenericEventArgs<IWebPort> args)
		{
			if (args.Data != null)
				Initialize();
		}

		#endregion
	}
}