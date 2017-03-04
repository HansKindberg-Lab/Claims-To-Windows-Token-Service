using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using HansKindberg.Abstractions;

namespace HansKindberg.Security.Principal
{
	[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is a wrapper.")]
	public class WindowsImpersonationContextWrapper : Wrapper<WindowsImpersonationContext>, IWindowsImpersonationContext
	{
		#region Constructors

		public WindowsImpersonationContextWrapper(WindowsImpersonationContext windowsImpersonationContext) : base(windowsImpersonationContext, nameof(windowsImpersonationContext)) {}

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is a wrapper.")]
		[SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "This is a wrapper.")]
		public virtual void Dispose()
		{
			this.WrappedInstance.Dispose();
		}

		public static WindowsImpersonationContextWrapper FromWindowsImpersonationContext(WindowsImpersonationContext windowsImpersonationContext)
		{
			return windowsImpersonationContext;
		}

		#region Implicit operator

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Should be disposed by the caller.")]
		public static implicit operator WindowsImpersonationContextWrapper(WindowsImpersonationContext windowsImpersonationContext)
		{
			return windowsImpersonationContext != null ? new WindowsImpersonationContextWrapper(windowsImpersonationContext) : null;
		}

		#endregion

		public virtual void Undo()
		{
			this.WrappedInstance.Undo();
		}

		#endregion
	}
}