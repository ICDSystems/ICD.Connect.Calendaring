using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Connect.Calendaring.Robin.Components.Events;
using ICD.Connect.Calendaring.Robin.Components.Users;

namespace ICD.Connect.Calendaring.Robin.Components
{
	public sealed class RobinServiceDeviceComponentFactory : IDisposable
	{
		private readonly Dictionary<Type, AbstractRobinServiceDeviceComponent> m_Components;
		private readonly SafeCriticalSection m_ComponentsSection;

		private static readonly Dictionary<Type, Func<RobinServiceDevice, AbstractRobinServiceDeviceComponent>> s_Factories =
			new Dictionary<Type, Func<RobinServiceDevice, AbstractRobinServiceDeviceComponent>>
			{
				{typeof(EventsComponent), robinRoom => new EventsComponent(robinRoom)},
				{typeof(UsersComponent), robinRoom => new UsersComponent(robinRoom)}
			};

		private readonly RobinServiceDevice m_RobinServiceDevice;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="robinRoom"></param>
		public RobinServiceDeviceComponentFactory(RobinServiceDevice robinRoom)
		{
			m_Components = new Dictionary<Type, AbstractRobinServiceDeviceComponent>();
			m_ComponentsSection = new SafeCriticalSection();

			m_RobinServiceDevice = robinRoom;
		}

		/// <summary>
		/// Deconstructor.
		/// </summary>
		~RobinServiceDeviceComponentFactory()
		{
			Dispose(false);
		}

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Gets the component with the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetComponent<T>()
			where T : AbstractRobinServiceDeviceComponent
		{

			Type key = typeof(T);

			m_ComponentsSection.Enter();

			try
			{
				AbstractRobinServiceDeviceComponent component;
				if (!m_Components.TryGetValue(key, out component))
				{
					component = s_Factories[key](m_RobinServiceDevice);
					m_Components.Add(key, component);
				}

				return component as T;
			}
			finally
			{
				m_ComponentsSection.Leave();
			}
		}

		/// <summary>
		/// Returns the cached components.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AbstractRobinServiceDeviceComponent> GetComponents()
		{
			return m_ComponentsSection.Execute(() => m_Components.Values.ToArray());
		}

		#endregion

		/// <summary>
		/// Release resources.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			m_ComponentsSection.Enter();

			try
			{
				foreach (AbstractRobinServiceDeviceComponent component in m_Components.Values)
					component.Dispose();
				m_Components.Clear();
			}
			finally
			{
				m_ComponentsSection.Leave();
			}
		}
	}
}