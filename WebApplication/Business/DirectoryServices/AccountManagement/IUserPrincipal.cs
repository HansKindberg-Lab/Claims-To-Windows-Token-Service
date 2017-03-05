using System;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	public interface IUserPrincipal : IDisposable
	{
		#region Properties

		string DistinguishedName { get; }
		string Notes { get; set; }
		string SamAccountName { get; }
		string UserPrincipalName { get; }

		#endregion
	}
}