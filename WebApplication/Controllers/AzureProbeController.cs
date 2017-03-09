using System.Web.Mvc;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers
{
	public class AzureProbeController : SiteController
	{
		#region Methods

		public virtual ActionResult Index()
		{
			return this.View(new AzureProbeViewModel());
		}

		#endregion
	}
}