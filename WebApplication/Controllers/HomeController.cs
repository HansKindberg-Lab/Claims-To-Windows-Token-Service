using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Web.Mvc;
using HansKindberg.Security.Principal;
using WebApplication.Business.DirectoryServices.AccountManagement;
using WebApplication.Business.DirectoryServices.AccountManagement.Extensions;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers
{
	public class HomeController : SiteController
	{
		#region Constructors

		public HomeController() : this(new UserPrincipalRepository(ConfigurationManager.ConnectionStrings["ActiveDirectory"].ConnectionString, new PrincipalContextParser()), new WindowsIdentityContext(), new WindowsIdentityFactory(), new WindowsImpersonator()) {}

		public HomeController(IUserPrincipalRepository userPrincipalRepository, IWindowsIdentityContext windowsIdentityContext, IWindowsIdentityFactory windowsIdentityFactory, IWindowsImpersonator windowsImpersonator)
		{
			if(userPrincipalRepository == null)
				throw new ArgumentNullException(nameof(userPrincipalRepository));

			if(windowsIdentityContext == null)
				throw new ArgumentNullException(nameof(windowsIdentityContext));

			if(windowsIdentityFactory == null)
				throw new ArgumentNullException(nameof(windowsIdentityFactory));

			if(windowsImpersonator == null)
				throw new ArgumentNullException(nameof(windowsImpersonator));

			this.UserPrincipalRepository = userPrincipalRepository;
			this.WindowsIdentityContext = windowsIdentityContext;
			this.WindowsIdentityFactory = windowsIdentityFactory;
			this.WindowsImpersonator = windowsImpersonator;
		}

		#endregion

		#region Properties

		protected internal virtual IPrincipal HttpContextUser
		{
			get
			{
				var windowsPrincipal = this.HttpContext.User as WindowsPrincipal;

				return windowsPrincipal != null ? (WindowsPrincipalWrapper) windowsPrincipal : this.HttpContext.User;
			}
		}

		protected internal virtual IUserPrincipalRepository UserPrincipalRepository { get; }
		protected internal virtual IWindowsIdentityContext WindowsIdentityContext { get; }
		protected internal virtual IWindowsIdentityFactory WindowsIdentityFactory { get; }
		protected internal virtual IWindowsImpersonator WindowsImpersonator { get; }

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected internal virtual string GetImpersonatedHttpContextUserIdentityInformation()
		{
			if(!this.HttpContextUser.Identity.IsAuthenticated)
				return "Error: The user is not authenticated.";

			if(!(this.HttpContextUser is IWindowsPrincipal))
				return "Error: The user is not a windows-user.";

			try
			{
				using(var userPrincipal = this.UserPrincipalRepository.Get(this.HttpContextUser))
				{
					if(userPrincipal == null)
						return "Error: The user-principal for \"" + this.HttpContextUser.Identity.Name + "\" does not exist.";

					using(var windowsIdentity = this.WindowsIdentityFactory.Create(userPrincipal.UserPrincipalName))
					{
						using(this.WindowsImpersonator.Impersonate(windowsIdentity))
						{
							using(var impersonatedWindowsIdentity = this.WindowsIdentityContext.Current)
							{
								string authenticationType;

								try
								{
									authenticationType = impersonatedWindowsIdentity.AuthenticationType;
								}
								catch(Exception exception)
								{
									authenticationType = "[Error getting authentication-type: " + exception + "]";
								}

								return "Impersonated windows-identity: \"" + impersonatedWindowsIdentity.Name + "\" (AuthenticationType = \"" + authenticationType + "\", ImpersonationLevel = " + impersonatedWindowsIdentity.ImpersonationLevel + ")";
							}
						}
					}
				}
			}
			catch(Exception exception)
			{
				return "Error: " + exception;
			}
		}

		public virtual ActionResult Index()
		{
			return this.View(new HomeViewModel(this.WindowsIdentityContext.Current, this.HttpContextUser) {ImpersonatedHttpContextUserIdentityInformation = this.GetImpersonatedHttpContextUserIdentityInformation()});
		}

		#endregion
	}
}