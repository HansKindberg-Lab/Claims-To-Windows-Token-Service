namespace HansKindberg.Security.Principal
{
	public interface ISecurityIdentifier : IIdentityReference
	{
		#region Properties

		int BinaryLength { get; }

		#endregion
	}
}