using System;
using System.Globalization;

namespace HansKindberg.Security.Principal
{
	public class WindowsImpersonator : IWindowsImpersonator
	{
		#region Methods

		public virtual IWindowsImpersonationContext Impersonate(IWindowsIdentity windowsIdentity)
		{
			if(windowsIdentity == null)
				throw new ArgumentNullException(nameof(windowsIdentity));

			var windowsIdentityInternal = windowsIdentity as IWindowsIdentityInternal;

			if(windowsIdentityInternal == null)
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The windows-identity must implement \"{0}\".", typeof(IWindowsIdentityInternal)), nameof(windowsIdentity));

			return windowsIdentityInternal.Impersonate();
		}

		#endregion
	}
}