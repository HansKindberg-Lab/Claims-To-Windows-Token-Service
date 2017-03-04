using System.Security.Principal;

namespace HansKindberg.Security.Principal
{
	public interface IWindowsPrincipal : IPrincipal
	{
		#region Properties

		IWindowsIdentity WindowsIdentity { get; }

		#endregion
	}
}