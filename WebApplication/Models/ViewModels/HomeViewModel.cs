using System;
using System.Security.Principal;
using HansKindberg.Security.Principal;

namespace WebApplication.Models.ViewModels
{
	public class HomeViewModel
	{
		#region Constructors

		public HomeViewModel(IWindowsIdentity currentWindowsIdentity, IPrincipal httpContextUser)
		{
			if(currentWindowsIdentity == null)
				throw new ArgumentNullException(nameof(currentWindowsIdentity));

			if(httpContextUser == null)
				throw new ArgumentNullException(nameof(httpContextUser));

			this.CurrentWindowsIdentity = currentWindowsIdentity;
			this.HttpContextUser = httpContextUser;
		}

		#endregion

		#region Properties

		public virtual IWindowsIdentity CurrentWindowsIdentity { get; }
		public virtual IPrincipal HttpContextUser { get; }
		public virtual string ImpersonatedHttpContextUserIdentityInformation { get; set; }
		public virtual string ImpersonatedHttpContextUserIdentityInformationWithClaimsToWindowsTokenService { get; set; }

		#endregion
	}
}