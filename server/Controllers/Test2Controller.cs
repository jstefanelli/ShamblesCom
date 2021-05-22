using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShamblesCom.Server.Controllers {
	public class Test2Controller : SPAController {
		public Test2Controller(IWebHostEnvironment env) : base(env) {

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
					{ "test", "some other value" },
					{ "test2", "another value "}
				}
			});
		}
	}
}