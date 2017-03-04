using System;

namespace HansKindberg.Abstractions
{
	public abstract class Wrapper : IWrapper
	{
		#region Constructors

		protected Wrapper(object wrappedInstance, string wrappedInstanceParameterName)
		{
			if(wrappedInstance == null)
				throw new ArgumentNullException(wrappedInstanceParameterName);

			this.WrappedInstance = wrappedInstance;
		}

		#endregion

		#region Properties

		public virtual object WrappedInstance { get; }

		#endregion
	}

	public abstract class Wrapper<T> : Wrapper, IWrapper<T>
	{
		#region Constructors

		protected Wrapper(T wrappedInstance, string wrappedInstanceParameterName) : base(wrappedInstance, wrappedInstanceParameterName) {}

		#endregion

		#region Properties

		public new virtual T WrappedInstance => (T) base.WrappedInstance;

		#endregion
	}
}