using System.Security.Principal;
using HansKindberg.Abstractions;

namespace HansKindberg.Security.Principal
{
	public class WindowsPrincipalWrapper : Wrapper<WindowsPrincipal>, IWindowsPrincipal
	{
		#region Constructors

		public WindowsPrincipalWrapper(WindowsPrincipal windowsPrincipal) : base(windowsPrincipal, nameof(windowsPrincipal)) {}

		#endregion

		#region Properties

		public virtual IIdentity Identity => this.WindowsIdentity;
		public virtual IWindowsIdentity WindowsIdentity => (WindowsIdentityWrapper) (WindowsIdentity) this.WrappedInstance.Identity;

		#endregion

		#region Methods

		public static WindowsPrincipalWrapper FromWindowsPrincipal(WindowsPrincipal windowsPrincipal)
		{
			return windowsPrincipal;
		}

		#endregion

		#region Implicit operator

		public static implicit operator WindowsPrincipalWrapper(WindowsPrincipal windowsPrincipal)
		{
			return windowsPrincipal != null ? new WindowsPrincipalWrapper(windowsPrincipal) : null;
		}

		public virtual bool IsInRole(string role)
		{
			return this.WrappedInstance.IsInRole(role);
		}

		#endregion
	}
}