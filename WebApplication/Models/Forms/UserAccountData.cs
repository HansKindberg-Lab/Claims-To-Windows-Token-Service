using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Forms
{
	public class UserAccountData
	{
		#region Properties

		public virtual bool ImpersonateWithClaimsToWindowsTokenService { get; set; }

		[Display(Name = "Notes")]
		public virtual string Notes { get; set; }

		#endregion
	}
}