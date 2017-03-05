﻿using System;
using System.DirectoryServices.AccountManagement;
using HansKindberg;
using HansKindberg.Abstractions;
using HansKindberg.Security.Principal;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	public class UserPrincipalRepository : IUserPrincipalRepository
	{
		#region Constructors

		public UserPrincipalRepository(string connectionString, IParser<IPrincipalContext> principalContextParser)
		{
			if(connectionString == null)
				throw new ArgumentNullException(nameof(connectionString));

			if(principalContextParser == null)
				throw new ArgumentNullException(nameof(principalContextParser));

			this.ConnectionString = connectionString;
			this.PrincipalContextParser = principalContextParser;
		}

		#endregion

		#region Properties

		protected internal virtual string ConnectionString { get; }
		protected internal virtual IParser<IPrincipalContext> PrincipalContextParser { get; }

		#endregion

		#region Methods

		protected internal virtual PrincipalContext CreatePrincipalContext()
		{
			return ((IWrapper<PrincipalContext>) this.PrincipalContextParser.Parse(this.ConnectionString)).WrappedInstance;
		}

		public virtual IUserPrincipal Get(ISecurityIdentifier securityIdentifier)
		{
			if(securityIdentifier == null)
				throw new ArgumentNullException(nameof(securityIdentifier));

			return (UserPrincipalWrapper) UserPrincipal.FindByIdentity(this.CreatePrincipalContext(), IdentityType.Sid, securityIdentifier.Value);
		}

		#endregion
	}
}