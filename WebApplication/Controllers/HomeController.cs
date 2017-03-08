using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using HansKindberg;
using HansKindberg.Security.Principal;
using WebApplication.Business;
using WebApplication.Business.DirectoryServices.AccountManagement;
using WebApplication.Business.DirectoryServices.AccountManagement.Extensions;
using WebApplication.Models.Forms;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers
{
	public class HomeController : SiteController
	{
		#region Constructors

		public HomeController() : this(new EnvironmentWrapper(), new SystemInformationFactory(), new UserPrincipalRepository(ConfigurationManager.ConnectionStrings["ActiveDirectory"].ConnectionString, new PrincipalContextParser()), new WindowsIdentityContext(), new WindowsIdentityFactory(), new WindowsImpersonator()) {}

		public HomeController(IEnvironment environment, ISystemInformationFactory systemInformationFactory, IUserPrincipalRepository userPrincipalRepository, IWindowsIdentityContext windowsIdentityContext, IWindowsIdentityFactory windowsIdentityFactory, IWindowsImpersonator windowsImpersonator)
		{
			if(environment == null)
				throw new ArgumentNullException(nameof(environment));

			if(systemInformationFactory == null)
				throw new ArgumentNullException(nameof(systemInformationFactory));

			if(userPrincipalRepository == null)
				throw new ArgumentNullException(nameof(userPrincipalRepository));

			if(windowsIdentityContext == null)
				throw new ArgumentNullException(nameof(windowsIdentityContext));

			if(windowsIdentityFactory == null)
				throw new ArgumentNullException(nameof(windowsIdentityFactory));

			if(windowsImpersonator == null)
				throw new ArgumentNullException(nameof(windowsImpersonator));

			this.Environment = environment;
			this.SystemInformationFactory = systemInformationFactory;
			this.UserPrincipalRepository = userPrincipalRepository;
			this.WindowsIdentityContext = windowsIdentityContext;
			this.WindowsIdentityFactory = windowsIdentityFactory;
			this.WindowsImpersonator = windowsImpersonator;
		}

		#endregion

		#region Properties

		protected internal virtual IEnvironment Environment { get; }

		protected internal virtual IPrincipal HttpContextUser
		{
			get
			{
				var windowsPrincipal = this.HttpContext.User as WindowsPrincipal;

				return windowsPrincipal != null ? (WindowsPrincipalWrapper) windowsPrincipal : this.HttpContext.User;
			}
		}

		protected internal virtual ISystemInformationFactory SystemInformationFactory { get; }
		protected internal virtual IUserPrincipalRepository UserPrincipalRepository { get; }
		protected internal virtual IWindowsIdentityContext WindowsIdentityContext { get; }
		protected internal virtual IWindowsIdentityFactory WindowsIdentityFactory { get; }
		protected internal virtual IWindowsImpersonator WindowsImpersonator { get; }

		#endregion

		#region Methods

		protected internal virtual HomeViewModel CreateModel()
		{
			return new HomeViewModel(this.WindowsIdentityContext.Current, this.Environment, this.HttpContextUser)
			{
				ImpersonatedHttpContextUserIdentityInformation = this.GetImpersonatedHttpContextUserIdentityInformation(),
				ImpersonatedHttpContextUserIdentityInformationWithClaimsToWindowsTokenService = this.GetImpersonatedHttpContextUserIdentityInformationWithClaimsToWindowsTokenService()
			};
		}

		protected internal virtual ActionResult CreateView(HomeViewModel model)
		{
			return this.View("~/Views/Home/Index.cshtml", model);
		}

		protected internal virtual IWindowsImpersonationContext CreateWindowsImpersonationContext(bool useClaimsToWindowsTokenService)
		{
			if(!this.HttpContextUser.Identity.IsAuthenticated)
				throw new InvalidOperationException("The user is not authenticated.");

			var windowsPrincipal = this.HttpContextUser as IWindowsPrincipal;

			if(windowsPrincipal == null)
				throw new InvalidOperationException("The user is not a windows-user.");

			if(useClaimsToWindowsTokenService)
			{
				using(var userPrincipal = this.UserPrincipalRepository.Get(this.HttpContextUser))
				{
					if(userPrincipal == null)
						throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The user-principal for \"{0}\" does not exist.", this.HttpContextUser.Identity.Name));

					var windowsIdentity = this.WindowsIdentityFactory.Create(userPrincipal.UserPrincipalName);

					return this.WindowsImpersonator.Impersonate(windowsIdentity);

					//using(var windowsIdentity = this.WindowsIdentityFactory.Create(userPrincipal.UserPrincipalName))
					//{
					//	return this.WindowsImpersonator.Impersonate(windowsIdentity);
					//}
				}
			}

			return this.WindowsImpersonator.Impersonate(windowsPrincipal.WindowsIdentity);
		}

		protected internal virtual string GetErrorMessage(ModelState modelState)
		{
			var error = modelState?.Errors.FirstOrDefault();

			if(error == null)
				return null;

			return error.Exception?.InnerException?.Message ?? error.ErrorMessage;
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected internal virtual string GetImpersonatedHttpContextUserIdentityInformation()
		{
			if(!this.HttpContextUser.Identity.IsAuthenticated)
				return "Error: The user is not authenticated.";

			var windowsPrincipal = this.HttpContextUser as IWindowsPrincipal;

			if(windowsPrincipal == null)
				return "Error: The user is not a windows-user.";

			try
			{
				using(this.WindowsImpersonator.Impersonate(windowsPrincipal.WindowsIdentity))
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
			catch(Exception exception)
			{
				return "Error: " + exception;
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected internal virtual string GetImpersonatedHttpContextUserIdentityInformationWithClaimsToWindowsTokenService()
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

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Function")]
		protected internal virtual void Impersonate(Func<Exception> function, bool useClaimsToWindowsTokenService)
		{
			if(function == null)
				throw new ArgumentNullException(nameof(function));

			using(this.CreateWindowsImpersonationContext(useClaimsToWindowsTokenService))
			{
				var exception = function.Invoke();

				if(exception != null)
					throw exception;
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual ActionResult Index()
		{
			var model = this.CreateModel();

			try
			{
				var userPrincipal = this.UserPrincipalRepository.Get(this.HttpContextUser);

				if(userPrincipal != null)
					model.UserAccountDataForm.Notes = userPrincipal.Notes;
			}
			// ReSharper disable EmptyGeneralCatchClause
			catch {}
			// ReSharper restore EmptyGeneralCatchClause

			return this.CreateView(model);
		}

		public virtual ActionResult SaveFile()
		{
			return this.RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual ActionResult SaveFile(FileData form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			var model = this.CreateModel();

			if(this.ValidateState(model))
			{
				try
				{
					this.Impersonate(() =>
					{
						try
						{
							return null;
						}
						catch(Exception exception)
						{
							return exception;
						}
					}, form.ImpersonateWithClaimsToWindowsTokenService);

					//this.SetConfirmation(model, "File saved.");
					model.SystemInformation = this.SystemInformationFactory.Create("Warning", "Not implemented", null, SystemInformationType.Warning);
				}
				catch(Exception exception)
				{
					this.SetError(model, exception);
				}
			}
			else
			{
				model.FileDataForm = form;
			}

			return this.CreateView(model);
		}

		public virtual ActionResult SaveUserAccount()
		{
			return this.RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual ActionResult SaveUserAccount(UserAccountData form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			var model = this.CreateModel();

			model.UserAccountDataForm = form;

			if(this.ValidateState(model))
			{
				try
				{
					this.Impersonate(() =>
					{
						try
						{
							var userPrincipal = this.UserPrincipalRepository.Get(this.HttpContextUser);

							if(userPrincipal == null)
								throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The user-principal for \"{0}\" does not exist.", this.HttpContextUser.Identity.Name));

							userPrincipal.Notes = form.Notes;

							this.UserPrincipalRepository.Save(userPrincipal);

							return null;
						}
						catch(Exception exception)
						{
							return exception;
						}
					}, form.ImpersonateWithClaimsToWindowsTokenService);

					this.SetConfirmation(model, "User-account saved.");
				}
				catch(Exception exception)
				{
					this.SetError(model, exception);
				}
			}

			return this.CreateView(model);
		}

		protected internal virtual void SetConfirmation(HomeViewModel model, string information)
		{
			if(model == null)
				throw new ArgumentNullException(nameof(model));

			model.SystemInformation = this.SystemInformationFactory.Create("Confirmation", information, null, SystemInformationType.Confirmation);
		}

		protected internal virtual void SetError(HomeViewModel model, Exception exception)
		{
			if(model == null)
				throw new ArgumentNullException(nameof(model));

			model.SystemInformation = this.SystemInformationFactory.Create("Error", exception?.ToString(), null, SystemInformationType.Exception);
		}

		protected internal virtual void SetInputError(HomeViewModel model, IEnumerable<string> details)
		{
			if(model == null)
				throw new ArgumentNullException(nameof(model));

			model.SystemInformation = this.SystemInformationFactory.Create("Error", "Input-error", details, SystemInformationType.Exception);
		}

		protected internal virtual bool ValidateState(HomeViewModel model)
		{
			if(this.ModelState.IsValid)
				return true;

			var details = this.ModelState.Where(modelState => modelState.Value.Errors.Any()).Select(modelState => string.Format(CultureInfo.InvariantCulture, "\"{0}\": {1}", modelState.Key, this.GetErrorMessage(modelState.Value))).ToArray();

			this.SetInputError(model, details);

			return false;
		}

		#endregion
	}
}