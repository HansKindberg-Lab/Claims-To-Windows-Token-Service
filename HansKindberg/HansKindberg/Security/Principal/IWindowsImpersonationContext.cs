using System;

namespace HansKindberg.Security.Principal
{
	public interface IWindowsImpersonationContext : IDisposable
	{
		#region Methods

		void Undo();

		#endregion
	}
}