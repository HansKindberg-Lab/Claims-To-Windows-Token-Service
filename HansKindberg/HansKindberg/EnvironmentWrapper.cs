using System;

namespace HansKindberg
{
	public class EnvironmentWrapper : IEnvironment
	{
		#region Properties

		public virtual string MachineName => Environment.MachineName;

		#endregion
	}
}