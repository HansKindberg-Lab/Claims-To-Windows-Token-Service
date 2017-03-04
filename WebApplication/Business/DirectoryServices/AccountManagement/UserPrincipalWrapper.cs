using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.AccountManagement;
using HansKindberg.Abstractions;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is a wrapper.")]
	public class UserPrincipalWrapper : Wrapper<UserPrincipal>, IUserPrincipal
	{
		#region Constructors

		public UserPrincipalWrapper(UserPrincipal userPrincipal) : base(userPrincipal, nameof(userPrincipal)) {}

		#endregion

		#region Properties

		public virtual string DistinguishedName => this.WrappedInstance.DistinguishedName;
		public virtual string SamAccountName => this.WrappedInstance.SamAccountName;
		public virtual string UserPrincipalName => this.WrappedInstance.UserPrincipalName;

		#endregion

		#region Methods

		public static UserPrincipalWrapper FromUserPrincipal(UserPrincipal userPrincipal)
		{
			return userPrincipal;
		}

		#endregion

		#region Implicit operator

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Should be disposed by the caller.")]
		public static implicit operator UserPrincipalWrapper(UserPrincipal userPrincipal)
		{
			return userPrincipal != null ? new UserPrincipalWrapper(userPrincipal) : null;
		}

		[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is a wrapper.")]
		[SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "This is a wrapper.")]
		public virtual void Dispose()
		{
			this.WrappedInstance.Dispose();
		}

		#endregion
	}
}