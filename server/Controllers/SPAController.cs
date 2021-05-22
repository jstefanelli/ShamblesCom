using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.Controllers {

	[ApiController]
	[Route("/[controller]")]
	public abstract class SPAController : ControllerBase {
		private IWebHostEnvironment Env { get; init; }

		public SPAController(IWebHostEnvironment env) {
			Env = env;
		}

		protected Task<ActionResult> HandleSPARequest() {
			if (Request.Headers.TryGetValue("X-SPA-Data", out StringValues s) && s.ToString() == "data"){
				return Task.FromResult<ActionResult>(null);
			}
			
			string filePath = Path.Combine(Env.ContentRootPath, "wwwroot", "index.html");
			return Task.FromResult<ActionResult>(new PhysicalFileResult(filePath, "text/html"));
		}
	}

	public class SPAData {
		[JsonPropertyName("view")]
		public string View { get; set; } = null;

		[JsonPropertyName("redraw")]
		public bool Redraw { get; set; } = false;

		[JsonPropertyName("data")]
		public Dictionary<string, object> Data { get; set; } = null;

		[JsonPropertyName("title")]
		public string Title { get; set; } = null;
	}
}