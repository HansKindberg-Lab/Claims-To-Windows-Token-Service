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

		#region Methods

		public override bool Equals(object obj)
		{
			var wrapper = obj as IWrapper;

			return this.WrappedInstance.Equals(wrapper != null ? wrapper.WrappedInstance : obj);
		}

		public override int GetHashCode()
		{
			return this.WrappedInstance.GetHashCode();
		}

		public override string ToString()
		{
			return this.WrappedInstance.ToString();
		}

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