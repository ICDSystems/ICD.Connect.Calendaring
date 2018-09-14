using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Connect.Calendaring.Robin.Components.Bookings;

namespace ICD.Connect.Calendaring.Robin.Components
{
	public sealed class RobinServiceDeviceComponentFactory : IDisposable
	{
		private readonly IcdHashSet<AbstractRobinServiceDeviceComponent> m_Components;
		private readonly SafeCriticalSection m_ComponentsSection;

		private static readonly Dictionary<Type, Func<RobinServiceDevice, AbstractRobinServiceDeviceComponent>> s_Factories =
			new Dictionary<Type, Func<RobinServiceDevice, AbstractRobinServiceDeviceComponent>>
			{
				{typeof(BookingsComponent), robinRoom => new BookingsComponent(robinRoom)}
			};

		private readonly RobinServiceDevice m_RobinServiceDevice;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="robinRoom"></param>
		public RobinServiceDeviceComponentFactory(RobinServiceDevice robinRoom)
		{
			m_Components = new IcdHashSet<AbstractRobinServiceDeviceComponent>();
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
			m_ComponentsSection.Enter();

			try
			{
				T output = m_Components.OfType<T>().FirstOrDefault() ?? s_Factories[typeof(T)](m_RobinServiceDevice) as T;
				m_Components.Add(output);

				return output;
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
			return m_ComponentsSection.Execute(() => m_Components.ToArray());
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
				foreach (AbstractRobinServiceDeviceComponent component in m_Components)
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