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
			return new JsonResult(new SPAData() {
				View = "user/login",
				Data = null
			});
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
				}
			}) /*{
				StatusCode = StatusCodes.Status401Unauthorized
			}*/;
		}
	}
}