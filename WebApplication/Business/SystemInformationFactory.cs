using System.Collections.Generic;
using WebApplication.Business.Collections.Generic.Extensions;

namespace WebApplication.Business
{
	public class SystemInformationFactory : ISystemInformationFactory
	{
		#region Methods

		public virtual ISystemInformation Create(string heading, string information, IEnumerable<string> detailedInformation, SystemInformationType type)
		{
			var systemInformation = new SystemInformation {Heading = heading, Information = information, Type = type};

			if(detailedInformation != null)
				systemInformation.DetailedInformation.Add(detailedInformation);

			return systemInformation;
		}

		#endregion
	}
}