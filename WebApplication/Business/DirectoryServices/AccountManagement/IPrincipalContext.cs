using System;
using System.DirectoryServices.AccountManagement;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	public interface IPrincipalContext : IDisposable
	{
		#region Properties

		string ConnectedServer { get; }
		string Container { get; }
		ContextType ContextType { get; }
		string Name { get; }
		ContextOptions Options { get; }
		string UserName { get; }

		#endregion
	}
}