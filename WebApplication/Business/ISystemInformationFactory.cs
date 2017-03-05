using System.Collections.Generic;

namespace WebApplication.Business
{
	public interface ISystemInformationFactory
	{
		#region Methods

		ISystemInformation Create(string heading, string information, IEnumerable<string> detailedInformation, SystemInformationType type);

		#endregion
	}
}