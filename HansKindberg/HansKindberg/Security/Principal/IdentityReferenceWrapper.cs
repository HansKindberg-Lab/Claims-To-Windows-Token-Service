using System;
using System.Security.Principal;
using HansKindberg.Abstractions;

namespace HansKindberg.Security.Principal
{
	public abstract class IdentityReferenceWrapper<T> : Wrapper<T>, IIdentityReference where T : IdentityReference
	{
		#region Constructors

		protected IdentityReferenceWrapper(T identityReference, string parameterName) : base(identityReference, parameterName) {}

		#endregion

		#region Properties

		public virtual string Value => this.WrappedInstance.Value;

		#endregion

		#region Methods

		public virtual IIdentityReference Translate(Type targetType)
		{
			return (IdentityReferenceWrapper) this.WrappedInstance.Translate(targetType);
		}

		#endregion
	}

	public class IdentityReferenceWrapper : IdentityReferenceWrapper<IdentityReference>
	{
		#region Constructors

		protected IdentityReferenceWrapper(IdentityReference identityReference) : base(identityReference, nameof(identityReference)) {}

		#endregion

		#region Methods

		public static IdentityReferenceWrapper FromIdentityReference(IdentityReference identityReference)
		{
			return identityReference;
		}

		#region Implicit operator

		public static implicit operator IdentityReferenceWrapper(IdentityReference identityReference)
		{
			return identityReference != null ? new IdentityReferenceWrapper(identityReference) : null;
		}

		#endregion

		#endregion
	}
}