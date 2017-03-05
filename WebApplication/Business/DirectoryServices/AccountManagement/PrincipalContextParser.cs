using System;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using HansKindberg;
using HansKindberg.Connections;

namespace WebApplication.Business.DirectoryServices.AccountManagement
{
	public class PrincipalContextParser : ConnectionStringParser, IParser<IPrincipalContext>
	{
		#region Fields

		private const string _containerKey = "Container";
		private const string _contextTypeKey = "ContextType";
		private const string _nameKey = "Name";
		private const string _optionsKey = "Options";
		private const string _passwordKey = "Password";
		private const string _userNameKey = "UserName";

		#endregion

		#region Properties

		protected internal virtual string ContainerKey => _containerKey;
		protected internal virtual string ContextTypeKey => _contextTypeKey;
		protected internal virtual string NameKey => _nameKey;
		protected internal virtual string OptionsKey => _optionsKey;
		protected internal virtual string PasswordKey => _passwordKey;
		protected internal virtual string UserNameKey => _userNameKey;

		#endregion

		#region Methods

		public virtual bool CanParse(string value)
		{
			IPrincipalContext principalContext;
			return this.TryParse(value, out principalContext);
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Should be disposed by the caller.")]
		public virtual IPrincipalContext Parse(string value)
		{
			if(value == null)
				return null;

			var dictionary = this.GetConnectionStringAsDictionary(value);

			var container = this.TryGetAndRemove(dictionary, this.ContainerKey);
			var contextType = (ContextType) Enum.Parse(typeof(ContextType), this.TryGetAndRemove(dictionary, this.ContextTypeKey), true);
			var name = this.TryGetAndRemove(dictionary, this.NameKey);

			ContextOptions? contextOptions = null;

			var optionsValue = this.TryGetAndRemove(dictionary, this.OptionsKey);
			if(optionsValue != null)
				contextOptions = (ContextOptions) Enum.Parse(typeof(ContextOptions), optionsValue, true);

			var password = this.TryGetAndRemove(dictionary, this.PasswordKey);
			var userName = this.TryGetAndRemove(dictionary, this.UserNameKey);

			if(dictionary.Keys.Any())
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The following keys are invalid: {0}.", string.Join(", ", dictionary.Keys.Select(key => "\"" + key + "\""))));

			var principalContext = contextOptions != null ? new PrincipalContext(contextType, name, container, contextOptions.Value, userName, password) : new PrincipalContext(contextType, name, container, userName, password);

			return (PrincipalContextWrapper) principalContext;
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual bool TryParse(string value, out IPrincipalContext result)
		{
			try
			{
				result = this.Parse(value);
				return true;
			}
			catch
			{
				result = null;
				return false;
			}
		}

		#endregion
	}
}