using System.Diagnostics.CodeAnalysis;
using Microsoft.IdentityModel.Claims;

namespace HansKindberg.Security.Principal
{
	public class WindowsIdentityFactory : IWindowsIdentityFactory
	{
		#region Fields

		private const string _defaultAuthenticationType = "Kerberos";
		private const bool _useWindowsTokenService = true;

		#endregion

		#region Properties

		protected internal virtual string DefaultAuthenticationType => _defaultAuthenticationType;
		protected internal virtual bool UseWindowsTokenService => _useWindowsTokenService;

		#endregion

		#region Methods

		public virtual IWindowsIdentity Create(string userPrincipalName)
		{
			return this.Create(userPrincipalName, this.DefaultAuthenticationType);
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Should be disposed by the caller.")]
		protected internal virtual IWindowsIdentity Create(string userPrincipalName, string authenticationType)
		{
			return (WindowsIdentityWrapper) WindowsClaimsIdentity.CreateFromUpn(userPrincipalName, authenticationType, this.UseWindowsTokenService);
		}

		#endregion
	}
}