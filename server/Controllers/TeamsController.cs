using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	[Route("/admin/{seasonId}/[controller]")]
	public class TeamsController : Controller {
		public ShamblesDBContext Db { get; set; }

		public TeamsController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet]
		[SPA]
		[Authorize]
		public async Task<ActionResult> Index(int seasonId) {
			

			return new JsonResult(new SPAData {
				View = "admin/teams",
				Data = new {
					Index = await AdminController.IndexData(Db, seasonId),
				},
				Url = $"/admin/{seasonId}/teams",
				Redraw = true
			});
		}
	}
}