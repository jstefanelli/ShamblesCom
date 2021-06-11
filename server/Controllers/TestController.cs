using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	[Route("/test")]
	public class TestController : Controller {
		public TestController() : base() {

		}

		[HttpGet]
		[SPA]
		public async Task<ActionResult> Get() {

			return new JsonResult(new SPAData() {
				View = "test",
				Redraw = false,
				Data = new Dictionary<string, object>{
					{ "test", "some value" },
					{ "test2", "another value "}
				}
			});
		}
	}
}