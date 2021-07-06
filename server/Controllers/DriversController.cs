using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ShamblesCom.Server.Controllers.Api;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.Models;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	
	[Route("/admin/{seasonId}/[controller]")]
	public class DriversController : Controller {
		public ShamblesDBContext Db { get; protected set; }

		public DriversController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet("")]
		[SPA]
		public async Task<ActionResult> Index(int seasonId) {
			var query = Db.Drivers.Include(d => d.RaceResults.OrderBy(rr => rr.Race.DateTime).Where(rr => rr.Race.SeasonId == seasonId))
				.Where(d => d.RaceResults.Where(rr => rr.Race.SeasonId == seasonId).Any());
			List<Driver> drivers = await query.ToListAsync();

			return new JsonResult(new SPAData {
				View = "admin/drivers",
				Data = new {
					index = await AdminController.IndexData(Db, seasonId),
					drivers = drivers
				},
				Url = $"admin/{seasonId}/drivers"
			});
		}
	}
}