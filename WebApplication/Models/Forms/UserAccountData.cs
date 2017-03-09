using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Forms
{
	public class UserAccountData
	{
		#region Properties

		[Display(Name = "Impersonate with Claims to Windows Token Service")]
		public virtual bool ImpersonateWithClaimsToWindowsTokenService { get; set; }

		[Display(Name = "Notes")]
		public virtual string Notes { get; set; }

		#endregion
	}
}