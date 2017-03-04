using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using HansKindberg.Abstractions;

namespace HansKindberg.Security.Principal
{
	public abstract class WindowsIdentityWrapper<T> : Wrapper<T>, IWindowsIdentity, IWindowsIdentityInternal where T : WindowsIdentity
	{
		#region Constructors

		protected WindowsIdentityWrapper(T windowsIdentity, string parameterName) : base(windowsIdentity, parameterName) {}

		#endregion

		#region Properties

		public virtual string AuthenticationType => this.WrappedInstance.AuthenticationType;
		public virtual TokenImpersonationLevel ImpersonationLevel => this.WrappedInstance.ImpersonationLevel;
		public virtual bool IsAnonymous => this.WrappedInstance.IsAnonymous;
		public virtual bool IsAuthenticated => this.WrappedInstance.IsAuthenticated;
		public virtual bool IsGuest => this.WrappedInstance.IsGuest;
		public virtual bool IsSystem => this.WrappedInstance.IsSystem;
		public virtual string Name => this.WrappedInstance.Name;

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is a wrapper.")]
		[SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "This is a wrapper.")]
		public virtual void Dispose()
		{
			this.WrappedInstance.Dispose();
		}

		public virtual IWindowsImpersonationContext Impersonate()
		{
			return (WindowsImpersonationContextWrapper) this.WrappedInstance.Impersonate();
		}

		#endregion
	}

	public class WindowsIdentityWrapper : WindowsIdentityWrapper<WindowsIdentity>
	{
		#region Constructors

		public WindowsIdentityWrapper(WindowsIdentity windowsIdentity) : base(windowsIdentity, nameof(windowsIdentity)) {}

		#endregion

		#region Methods

		public static WindowsIdentityWrapper FromWindowsIdentity(WindowsIdentity windowsIdentity)
		{
			return windowsIdentity;
		}

		#region Implicit operator

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Should be disposed by the caller.")]
		public static implicit operator WindowsIdentityWrapper(WindowsIdentity windowsIdentity)
		{
			return windowsIdentity != null ? new WindowsIdentityWrapper(windowsIdentity) : null;
		}

		#endregion

		#endregion
	}
}