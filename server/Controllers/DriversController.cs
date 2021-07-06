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
using ShamblesCom.Server.Controllers.Data;

namespace ShamblesCom.Server.Controllers {
	
	[Route("/admin/{seasonId}/[controller]")]
	public class DriversController : Controller {
		public ShamblesDBContext Db { get; protected set; }
		public DriversController(ShamblesDBContext db) {
			Db = db;
		}

		private static int CompareDrivers(DriverInfo d0, DriverInfo d1, int pos = 1) {
			if (d0.SeasonPoints != d1.SeasonPoints) {
				return d0.SeasonPoints - d1.SeasonPoints;
			}

			if (pos >= 50)
				return 0;

			int d0p = d0.Driver.RaceResults.Where(rr => rr.Position == pos && rr.Finished).Count();
			int d1p = d1.Driver.RaceResults.Where(rr => rr.Position == pos && rr.Finished).Count();

			if (d0p - d1p == 0) {
				return CompareDrivers(d0, d1, pos + 1);
			}

			return d0p - d1p;
		}

		[HttpGet("")]
		[SPA]
		public async Task<ActionResult> Index(int seasonId) {
			var query = Db.Drivers
				.Include(d => d.RaceResults.Where(rr => rr.Race.SeasonId == seasonId))
				.Where(d => d.RaceResults.Where(rr => rr.Race.SeasonId == seasonId).Any())
				.Select(d => new DriverInfo{ Driver = d, SeasonPoints = d.RaceResults.Where(rr => rr.Race.SeasonId == seasonId).Sum(rr => rr.Points) })
				.OrderByDescending(d => d.SeasonPoints);

			List<DriverInfo> drivers = await query.ToListAsync();
			
			drivers.Sort((d0, d1) => CompareDrivers(d0, d1));

			for (int i = 0; i < drivers.Count; i++) {
				drivers[i].SeasonPoints = i + 1;
			}

			return new JsonResult(new SPAData {
				View = "admin/drivers",
				Data = new {
					index = await AdminController.IndexData(Db, seasonId),
					season = await Db.Seasons.FindAsync(seasonId),
					drivers = drivers
				},
				Url = $"admin/{seasonId}/drivers"
			});
		}
		
		[ValidateModel]
		[SPA]
		[HttpPost("")]
		public async Task<ActionResult> AddDriver(int seasonId, [FromBody] Driver driver) {
			if (await Db.Drivers.Where(d => d.Nickname == driver.Nickname).AnyAsync()) {
				ModelState.AddModelError("Nickname", "A driver with this nickname already exists");
			}

			if (!ModelState.IsValid) {
				return new BadRequestObjectResult(ModelState);
			}

			Db.Drivers.Add(driver);
			await Db.SaveChangesAsync();

			return new SeeOtherResult($"/admin/{seasonId}/drivers");
		}
	}
}