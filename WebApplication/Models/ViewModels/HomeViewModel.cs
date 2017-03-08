using System;
using System.Security.Principal;
using HansKindberg;
using HansKindberg.Security.Principal;
using WebApplication.Business;
using WebApplication.Models.Forms;

namespace WebApplication.Models.ViewModels
{
	public class HomeViewModel
	{
		#region Fields

		private FileData _fileDataForm;
		private UserAccountData _userAccountDataForm;

		#endregion

		#region Constructors

		public HomeViewModel(IWindowsIdentity currentWindowsIdentity, IEnvironment environment, IPrincipal httpContextUser)
		{
			if(currentWindowsIdentity == null)
				throw new ArgumentNullException(nameof(currentWindowsIdentity));

			if(environment == null)
				throw new ArgumentNullException(nameof(environment));

			if(httpContextUser == null)
				throw new ArgumentNullException(nameof(httpContextUser));

			this.CurrentWindowsIdentity = currentWindowsIdentity;
			this.Environment = environment;
			this.HttpContextUser = httpContextUser;
		}

		#endregion

		#region Properties

		public virtual IWindowsIdentity CurrentWindowsIdentity { get; }
		public virtual IEnvironment Environment { get; }

		public virtual FileData FileDataForm
		{
			get { return this._fileDataForm ?? (this._fileDataForm = new FileData()); }
			set { this._fileDataForm = value; }
		}

		public virtual IPrincipal HttpContextUser { get; }
		public virtual string ImpersonatedHttpContextUserIdentityInformation { get; set; }
		public virtual string ImpersonatedHttpContextUserIdentityInformationWithClaimsToWindowsTokenService { get; set; }
		public virtual ISystemInformation SystemInformation { get; set; }

		public virtual UserAccountData UserAccountDataForm
		{
			get { return this._userAccountDataForm ?? (this._userAccountDataForm = new UserAccountData()); }
			set { this._userAccountDataForm = value; }
		}

		#endregion
	}
}