namespace HansKindberg.Security.Principal
{
	public interface IWindowsImpersonator
	{
		#region Methods

		IWindowsImpersonationContext Impersonate(IWindowsIdentity windowsIdentity);

		#endregion
	}
}