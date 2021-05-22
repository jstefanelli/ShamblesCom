using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShamblesCom.Server.Controllers {
	public class TestController : SPAController {
		public TestController(IWebHostEnvironment env) : base(env) {

		}

		[HttpGet]
		[ResponseCache(VaryByHeader = "X-SPA-Data", Duration = 30, Location = ResponseCacheLocation.Client)]
		public async Task<ActionResult> Get() {
			ActionResult res = await HandleSPARequest();

			if (res != null)
				return res;

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