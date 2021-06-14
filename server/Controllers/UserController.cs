using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	public class UserController : ControllerBase {
		protected ShamblesDBContext Db { get; init; }

		public UserController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet("/login")]
		[SPA]
		public async Task<ActionResult> Loginpage() {
			if (HttpContext.User?.Identity?.IsAuthenticated == true) {
				return Redirect("/admin");
			}

			return new JsonResult(new SPAData() {
				View = "user/login",
				Data = null,
				Url = "/login"
			});
		}

		[HttpPost("/logout")]
		[HttpGet("/logout")]
		[SPA]
		public async Task<ActionResult> Logout() {
			if (HttpContext.User?.Identity.IsAuthenticated == true) {
				await UserManager.Logout(HttpContext);
			}

			return Redirect("/homepage");
		}

		[HttpPost("/login")]
		[SPA]
		public async Task<ActionResult> Login(string email, string password) {
			if (await UserManager.Login(HttpContext, Db, email, password) != null) {
				return Redirect("/admin");
			}

			return new JsonResult(new SPAData() {
				View = "user/login",
				Data = new {
					errors = new {
						name = "Invalid email or password"
					}
				},
				Url = "/login"
			}) /*{
				StatusCode = StatusCodes.Status401Unauthorized
			}*/;
		}
	}
}