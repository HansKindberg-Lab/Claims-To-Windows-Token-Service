using System.Diagnostics.CodeAnalysis;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	public interface IUserPrincipalRepository
	{
		#region Methods

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		IUserPrincipal Get(string samAccountName);

		#endregion
	}
}