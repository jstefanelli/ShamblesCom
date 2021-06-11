using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	[Route("/")]
	public class IndexController : Controller {
		public IndexController() : base() {

		}

		[HttpGet]
		[SPA]
		[ResponseCache(VaryByHeader = "X-SPA-Data", Duration = 30, Location = ResponseCacheLocation.Client)]
		public async Task<ActionResult> Get() {
			return new JsonResult(new SPAData() {
				View = "index",
				Redraw = false,
				Data = null
			});

		}
	}
}