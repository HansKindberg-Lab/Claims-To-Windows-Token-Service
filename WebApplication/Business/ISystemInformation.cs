using System.Collections.Generic;

namespace WebApplication.Business
{
	public interface ISystemInformation
	{
		#region Properties

		IEnumerable<string> DetailedInformation { get; }
		string Heading { get; }
		string Information { get; }
		SystemInformationType Type { get; }

		#endregion
	}
}