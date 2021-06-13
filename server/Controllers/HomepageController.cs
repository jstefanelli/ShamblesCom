using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ShamblesCom.Server.SPA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ShamblesCom.Server.Controllers {
	[Route("/homepage")]
	public class HomepageController : Controller {
		[HttpGet]
		[SPA]
		public async Task<ActionResult> Get() {
			/*await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new [] {
				new Claim(ClaimTypes.Email, "john@jstefanelli.com"),
				new Claim(ClaimTypes.Name, "John Stefanelli")
			}, CookieAuthenticationDefaults.AuthenticationScheme)), new AuthenticationProperties() {
				AllowRefresh  = true,
			});*/
			await HttpContext.SignOutAsync();


			return new JsonResult(new SPAData() {
				View = "homepage",
				Redraw = false,
				Data = null
			});

		}
	}
}