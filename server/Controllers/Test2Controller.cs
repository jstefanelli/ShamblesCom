using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	[Route("/test2")]
	public class Test2Controller : Controller {
		public Test2Controller() : base() {

		}

		[HttpGet]
		[SPA]
		public async Task<ActionResult> Get() {

			return new JsonResult(new SPAData() {
				View = "test",
				Redraw = false,
				Data = new Dictionary<string, object>{
					{ "test", "some other value" },
					{ "test2", "another value "}
				}
			});
		}
	}
}