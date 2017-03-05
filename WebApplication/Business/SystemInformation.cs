using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication.Business
{
	public class SystemInformation : ISystemInformation
	{
		#region Properties

		public virtual IList<string> DetailedInformation { get; } = new List<string>();
		IEnumerable<string> ISystemInformation.DetailedInformation => this.DetailedInformation;
		public virtual string Heading { get; set; }
		public virtual string Information { get; set; }

		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		public virtual SystemInformationType Type { get; set; }

		#endregion
	}
}