namespace HansKindberg.Security.Principal
{
	public interface IWindowsIdentityContext
	{
		#region Properties

		IWindowsIdentity Anonymous { get; }
		IWindowsIdentity Current { get; }

		#endregion
	}
}