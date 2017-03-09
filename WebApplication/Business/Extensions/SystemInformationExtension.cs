using System;

namespace WebApplication.Business.Extensions
{
	public static class SystemInformationExtension
	{
		#region Methods

		public static string GetAlertClass(this ISystemInformation systemInformation, string confirmationClass, string exceptionClass, string informationClass, string warningClass)
		{
			if(systemInformation == null)
				throw new ArgumentNullException(nameof(systemInformation));

			// ReSharper disable SwitchStatementMissingSomeCases
			switch(systemInformation.Type)
			{
				case SystemInformationType.Confirmation:
					return confirmationClass;
				case SystemInformationType.Exception:
					return exceptionClass;
				case SystemInformationType.Warning:
					return warningClass;
				default:
					return informationClass;
			}
			// ReSharper restore SwitchStatementMissingSomeCases
		}

		#endregion
	}
}