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
		public async Task<ActionResult> Get() {
			return new JsonResult(new SPAData() {
				View = "index",
				Redraw = false,
				Data = null
			});

		}
	}
}