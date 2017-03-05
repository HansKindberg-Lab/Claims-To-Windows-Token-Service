using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using HansKindberg.Abstractions;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is a wrapper.")]
	public class UserPrincipalWrapper : Wrapper<UserPrincipal>, IUserPrincipal
	{
		#region Fields

		private static readonly MethodInfo _extensionGetMethod = typeof(Principal).GetMethod("ExtensionGet", BindingFlags.Instance | BindingFlags.NonPublic);
		private static readonly MethodInfo _extensionSetMethod = typeof(Principal).GetMethod("ExtensionSet", BindingFlags.Instance | BindingFlags.NonPublic);
		private const string _notesAttributeName = "info";

		#endregion

		#region Constructors

		public UserPrincipalWrapper(UserPrincipal userPrincipal) : base(userPrincipal, nameof(userPrincipal)) {}

		#endregion

		#region Properties

		public virtual string DistinguishedName => this.WrappedInstance.DistinguishedName;

		public virtual string Notes
		{
			get
			{
				var notes = this.ExtensionGet(this.NotesAttributeName);

				if(!notes.Any())
					return null;

				return notes.First() as string;
			}
			set { this.ExtensionSet(this.NotesAttributeName, value); }
		}

		protected internal virtual string NotesAttributeName => _notesAttributeName;
		public virtual string SamAccountName => this.WrappedInstance.SamAccountName;
		public virtual string UserPrincipalName => this.WrappedInstance.UserPrincipalName;

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is a wrapper.")]
		[SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "This is a wrapper.")]
		public virtual void Dispose()
		{
			this.WrappedInstance.Dispose();
		}

		protected internal virtual object[] ExtensionGet(string attribute)
		{
			return (object[]) _extensionGetMethod.Invoke(this.WrappedInstance, new object[] {attribute});
		}

		protected internal virtual void ExtensionSet(string attribute, object value)
		{
			_extensionSetMethod.Invoke(this.WrappedInstance, new[] {attribute, value});
		}

		public static UserPrincipalWrapper FromUserPrincipal(UserPrincipal userPrincipal)
		{
			return userPrincipal;
		}

		#region Implicit operator

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Should be disposed by the caller.")]
		public static implicit operator UserPrincipalWrapper(UserPrincipal userPrincipal)
		{
			return userPrincipal != null ? new UserPrincipalWrapper(userPrincipal) : null;
		}

		#endregion

		#endregion
	}
}