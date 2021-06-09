using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShamblesCom.Server.Controllers {
	[Route("/")]
	public class IndexController : SPAController {
		public IndexController(IWebHostEnvironment env) : base(env) {

		}

		[HttpGet]
		[ResponseCache(VaryByHeader = "X-SPA-Data", Duration = 30, Location = ResponseCacheLocation.Client)]
		public async Task<ActionResult> Get() {
			ActionResult res = await HandleSPARequest();

			if (res != null) {
				return res;
			}

			return new JsonResult(new SPAData() {
				View = "index",
				Redraw = false,
				Data = null
			});

		}
	}
}