namespace HansKindberg.Security.Principal
{
	public interface IWindowsIdentityInternal
	{
		#region Methods

		IWindowsImpersonationContext Impersonate();

		#endregion
	}
}