using System;
using System.Security.Principal;
using HansKindberg.Security.Principal;

namespace WebApplication.Business.DirectoryServices.AccountManagement.Extensions
{
	public static class UserPrincipalRepositoryExtension
	{
		#region Methods

		public static IUserPrincipal Get(this IUserPrincipalRepository userPrincipalRepository, IPrincipal principal)
		{
			if(userPrincipalRepository == null)
				throw new ArgumentNullException(nameof(userPrincipalRepository));

			if(principal == null)
				throw new ArgumentNullException(nameof(principal));

			var windowsIdentity = (principal as IWindowsPrincipal)?.WindowsIdentity;

			if(windowsIdentity == null || !windowsIdentity.IsAuthenticated)
				return null;

			return userPrincipalRepository.Get(windowsIdentity.User);
		}

		#endregion
	}
}