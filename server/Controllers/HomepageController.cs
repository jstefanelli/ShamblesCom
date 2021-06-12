using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	[Route("/homepage")]
	public class HomepageController : Controller {


		[HttpGet]
		[SPA]
		public async Task<ActionResult> Get() {
			return new JsonResult(new SPAData() {
				View = "homepage",
				Redraw = false,
				Data = null
			});

		}
	}
}