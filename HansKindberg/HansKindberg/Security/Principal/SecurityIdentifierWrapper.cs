using System.Security.Principal;

namespace HansKindberg.Security.Principal
{
	public class SecurityIdentifierWrapper : IdentityReferenceWrapper<SecurityIdentifier>, ISecurityIdentifier
	{
		#region Constructors

		public SecurityIdentifierWrapper(SecurityIdentifier securityIdentifier) : base(securityIdentifier, nameof(securityIdentifier)) {}

		#endregion

		#region Properties

		public virtual int BinaryLength => this.WrappedInstance.BinaryLength;

		#endregion

		#region Methods

		public static SecurityIdentifierWrapper FromSecurityIdentifier(SecurityIdentifier securityIdentifier)
		{
			return securityIdentifier;
		}

		#region Implicit operator

		public static implicit operator SecurityIdentifierWrapper(SecurityIdentifier securityIdentifier)
		{
			return securityIdentifier != null ? new SecurityIdentifierWrapper(securityIdentifier) : null;
		}

		#endregion

		#endregion
	}
}