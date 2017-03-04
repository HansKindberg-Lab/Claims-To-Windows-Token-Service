using System.Security.Principal;

namespace HansKindberg.Security.Principal
{
	public class WindowsIdentityContext : IWindowsIdentityContext
	{
		#region Properties

		public virtual IWindowsIdentity Anonymous => (WindowsIdentityWrapper) WindowsIdentity.GetAnonymous();
		public virtual IWindowsIdentity Current => (WindowsIdentityWrapper) WindowsIdentity.GetCurrent();

		#endregion
	}
}