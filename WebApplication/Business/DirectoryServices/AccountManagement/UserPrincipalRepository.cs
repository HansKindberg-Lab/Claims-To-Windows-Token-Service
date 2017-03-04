using System.DirectoryServices.AccountManagement;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	public class UserPrincipalRepository : IUserPrincipalRepository
	{
		#region Methods

		public virtual IUserPrincipal Get(string samAccountName)
		{
			return (UserPrincipalWrapper) UserPrincipal.FindByIdentity(UserPrincipal.Current.Context, IdentityType.SamAccountName, samAccountName);
		}

		#endregion
	}
}