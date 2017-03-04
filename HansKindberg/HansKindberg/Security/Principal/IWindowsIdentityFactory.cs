namespace HansKindberg.Security.Principal
{
	public interface IWindowsIdentityFactory
	{
		#region Methods

		IWindowsIdentity Create(string userPrincipalName);

		#endregion
	}
}