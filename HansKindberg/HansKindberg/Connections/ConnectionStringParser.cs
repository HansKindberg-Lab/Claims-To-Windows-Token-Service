using System;
using System.Collections.Generic;

namespace HansKindberg.Connections
{
	public abstract class ConnectionStringParser
	{
		#region Constructors

		protected ConnectionStringParser() : this(';', '=', StringComparer.OrdinalIgnoreCase) {}

		protected ConnectionStringParser(char parameterDelimiter, char nameValueDelimiter, IEqualityComparer<string> nameComparer)
		{
			if(nameComparer == null)
				throw new ArgumentNullException(nameof(nameComparer));

			this.NameComparer = nameComparer;
			this.NameValueDelimiter = nameValueDelimiter;
			this.ParameterDelimiter = parameterDelimiter;
		}

		#endregion

		#region Properties

		public virtual IEqualityComparer<string> NameComparer { get; }
		public virtual char NameValueDelimiter { get; }
		public virtual char ParameterDelimiter { get; }

		#endregion

		#region Methods

		protected internal virtual IDictionary<string, string> GetConnectionStringAsDictionary(string connectionString)
		{
			var dictionary = new Dictionary<string, string>(this.NameComparer);

			// ReSharper disable InvertIf
			if(!string.IsNullOrEmpty(connectionString))
			{
				foreach(var nameValue in connectionString.Split(this.ParameterDelimiter))
				{
					var nameValueParts = nameValue.Split(new[] {this.NameValueDelimiter}, 2);

					if(nameValueParts.Length == 0)
						continue;

					var value = nameValueParts.Length > 1 ? nameValueParts[1] : string.Empty;

					dictionary.Add(nameValueParts[0], value);
				}
			}
			// ReSharper restore InvertIf

			return dictionary;
		}

		protected internal virtual string TryGetAndRemove(IDictionary<string, string> dictionary, string key)
		{
			string value = null;

			// ReSharper disable InvertIf
			if(dictionary != null && key != null && dictionary.ContainsKey(key))
			{
				value = dictionary[key];

				dictionary.Remove(key);
			}
			// ReSharper restore InvertIf

			return value;
		}

		#endregion
	}
}