using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace HansKindberg.Configuration
{
	public interface IConfigurationManager
	{
		#region Properties

		NameValueCollection ApplicationSettings { get; }
		IEnumerable<ConnectionStringSettings> ConnectionStrings { get; }

		#endregion

		#region Methods

		object GetSection(string sectionName);
		void RefreshSection(string sectionName);

		#endregion
	}
}