using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	[Route("/admin")]
	public class AdminController : Controller {
		protected ShamblesDBContext Db { get; init; }

		public AdminController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet]
		[SPA]
		[Authorize]
		public async Task<ActionResult> Index() {
			return new JsonResult(new SPAData() {
				View = "admin/home",
				Data = new {
					seasons = Db.Seasons.ToArray(),
					games = Db.Games.ToArray(),
					categories = Db.Categories.ToArray()
				},
				Url = "/admin"
			});
		}
	}
}