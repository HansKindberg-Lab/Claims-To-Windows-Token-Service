using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.AccountManagement;
using HansKindberg.Abstractions;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is a wrapper.")]
	public class PrincipalContextWrapper : Wrapper<PrincipalContext>, IPrincipalContext
	{
		#region Constructors

		public PrincipalContextWrapper(PrincipalContext principalContext) : base(principalContext, nameof(principalContext)) {}

		#endregion

		#region Properties

		public virtual string ConnectedServer => this.WrappedInstance.ConnectedServer;
		public virtual string Container => this.WrappedInstance.Container;
		public virtual ContextType ContextType => this.WrappedInstance.ContextType;
		public virtual string Name => this.WrappedInstance.Name;
		public virtual ContextOptions Options => this.WrappedInstance.Options;
		public virtual string UserName => this.WrappedInstance.UserName;

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is a wrapper.")]
		[SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "This is a wrapper.")]
		public virtual void Dispose()
		{
			this.WrappedInstance.Dispose();
		}

		public static PrincipalContextWrapper FromPrincipalContext(PrincipalContext principalContext)
		{
			return principalContext;
		}

		#region Implicit operator

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Should be disposed by the caller.")]
		public static implicit operator PrincipalContextWrapper(PrincipalContext principalContext)
		{
			return principalContext != null ? new PrincipalContextWrapper(principalContext) : null;
		}

		#endregion

		#endregion
	}
}