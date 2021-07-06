using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ShamblesCom.Server.SPA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ShamblesCom.Server.Controllers {
	[Route("/homepage")]
	public class HomepageController : ControllerBase {
		[HttpGet]
		[SPA]
		public Task<ActionResult> Get() {
			return Task.FromResult<ActionResult>(new JsonResult(new SPAData() {
				View = "homepage",
				Redraw = false,
				Data = null,
				Url = "/homepage"
			}));

		}
	}
}