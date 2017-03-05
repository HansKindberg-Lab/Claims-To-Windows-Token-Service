using System;

namespace HansKindberg.Security.Principal
{
	public interface IIdentityReference
	{
		#region Properties

		string Value { get; }

		#endregion

		#region Methods

		IIdentityReference Translate(Type targetType);

		#endregion
	}
}