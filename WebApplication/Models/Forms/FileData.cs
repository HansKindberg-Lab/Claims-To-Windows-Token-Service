using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Forms
{
	public class FileData
	{
		#region Properties

		public virtual bool ImpersonateWithClaimsToWindowsTokenService { get; set; }

		[Display(Name = "Line to add")]
		public virtual string LineToAdd { get; set; }

		[Display(Name = "Path")]
		public virtual string Path { get; set; }

		#endregion
	}
}