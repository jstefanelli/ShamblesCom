using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace ShamblesCom.Server.SPA {
	public class SPAMiddleware {
		protected RequestDelegate Next { get; init; }
		protected IWebHostEnvironment Env { get; init; }

		public SPAMiddleware (RequestDelegate next, IWebHostEnvironment env) {
			Next = next;
			Env = env;
		}

		public async Task InvokeAsync(HttpContext context) {
			if (context.GetEndpoint()?.Metadata?.GetMetadata<SPAAttribute>() != null) {
				Console.WriteLine($"SPA Middleware has header: {context.Request.Headers.ContainsKey("X-SPA-Data")}");
				if (context.Request.Headers.TryGetValue("X-SPA-Data", out StringValues spa) && spa.ToString() == "data") {
					Console.WriteLine("SPA Middleware found header");
					await Next(context);
					return;
				}
				
				string filePath = Path.Combine(Env.ContentRootPath, "wwwroot", "index.html");
				context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue() {
					Public = false,
					MaxAge = TimeSpan.FromSeconds(0)
				};
				await new PhysicalFileResult(filePath, "text/html").ExecuteResultAsync(new ActionContext() {
					HttpContext = context
				});
				return;
			}

			await Next(context);
		}

		public static void UseSPAMiddlware(IApplicationBuilder app) {
			app.UseMiddleware<SPAMiddleware>();
		}
	}

}