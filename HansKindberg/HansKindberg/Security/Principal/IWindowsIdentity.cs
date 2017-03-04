using System;
using System.Security.Principal;

namespace HansKindberg.Security.Principal
{
	public interface IWindowsIdentity : IDisposable, IIdentity
	{
		#region Properties

		TokenImpersonationLevel ImpersonationLevel { get; }
		bool IsAnonymous { get; }
		bool IsGuest { get; }
		bool IsSystem { get; }

		#endregion
	}
}