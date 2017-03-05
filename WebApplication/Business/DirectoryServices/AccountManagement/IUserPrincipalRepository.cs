using System.Diagnostics.CodeAnalysis;
using HansKindberg.Security.Principal;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	public interface IUserPrincipalRepository
	{
		#region Methods

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		IUserPrincipal Get(ISecurityIdentifier securityIdentifier);

		#endregion
	}
}