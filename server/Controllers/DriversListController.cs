using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.Controllers.Api;
using ShamblesCom.Server.Controllers.Data;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.DTO;
using ShamblesCom.Server.DB.Models;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	[Route("/drivers")]
	public class DriversListController : Controller {
		protected ShamblesDBContext Db { get; init; }

		public DriversListController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet]
		[SPA]
		public async Task<ActionResult> Index(int seasonId = -1) {
			if (seasonId == -1) {
				SiteSettings settings = await Db.Settings.FirstOrDefaultAsync();
				if (settings == null) {
					return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
				}

				seasonId = settings.CurrentHomeSeasonId ?? 1;
			}
			
			List<Driver> seasonDrivers = new List<Driver>();

			Season currentSeason = await Db.Seasons.Include(s => s.Races).ThenInclude(r => r.RaceResults).Where(s => s.Id == seasonId).FirstOrDefaultAsync();
			if (currentSeason == null) {
				return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
			}
			List<DTODriverInfo> profiles = await Db.Profiles.Where(p => p.SeasonId == currentSeason.Id)
				.Include(p => p.Driver)
					.ThenInclude(d => d.RaceResults.Where(rr => rr.Race.SeasonId == currentSeason.Id))
						.ThenInclude(rr => rr.Race)
				.Select(p => new DTODriverInfo(new DTODriver(p.Driver) {
					RaceResults = p.Driver.RaceResults.Select(rr => new DTORaceResult(rr) {
						Race = new DTORace(rr.Race)
					}).ToList()
				}) {
					Profile = new DTODriverProfile(p),
					SeasonPoints = p.Driver.RaceResults.Where(r => r.Race.SeasonId == seasonId).Sum(rr => rr.Points),
				})
				.ToListAsync();

			foreach(var i in profiles) {
				int maxTeamId = 0;
				int maxTeamAmount = 0;
				foreach(int teamId in i.Driver.RaceResults.Where(rr => rr.Race?.SeasonId == seasonId).Select(t => t.TeamId).Distinct()) {
					int amount = i.Driver.RaceResults.Where(rr => rr.TeamId == teamId).Count();

					if (amount > maxTeamAmount) {
						maxTeamId = teamId;
						maxTeamAmount = amount;
					}
				}
				
				if (maxTeamId > 0) {
					i.MostCommonTeam = new DTOTeam(await Db.Teams.FindAsync(maxTeamId));
				}
			}

			profiles.Sort();

			foreach(var p in profiles) {
				if (p != null && p.Driver != null) {
					p.Driver.RaceResults = null;
				}
			}

			var seasonTask = Task.Run(() => { 
				return new DTOSeason(currentSeason) {
					Races = currentSeason.Races.Select(r => new DTORace(r) {
						RaceResults = r.RaceResults.Select(rr => new DTORaceResult(rr)).ToList()
					}).ToList()
				};
			});

			var seasonsTask = Task.Run(() => {
				return Db.Seasons.Where(s => s.Id != currentSeason.Id).Select(s => new DTOSeason(s)).ToList();
			});

			return new JsonResult(new SPAData() {
				View = "drivers/index",
				Data = new {
					profiles = profiles,
					season = await seasonTask,
					seasons = await seasonsTask
				}
			});
		}

		[HttpGet("{driverId:int}")]
		[SPA]
		public async Task<ActionResult> Details(int driverId, [FromQuery] int seasonId = -1) {
			if (seasonId == -1) {
				seasonId = (await Db.Settings.FirstOrDefaultAsync())?.CurrentHomeSeasonId ?? throw new Exception("No default season ID");
			}

			Season s = await Db.Seasons.FindAsync(seasonId);
			DriverProfile p = await Db.Profiles.Where(p => p.DriverId == driverId && p.SeasonId == seasonId).Include(p => p.Driver).FirstOrDefaultAsync();

			int mainTeamId = (await Db.RaceResults.Where(rr => rr.Race.SeasonId == seasonId && rr.DriverId == driverId).ToListAsync())
				.GroupBy(rr => rr.TeamId).Select(g => new { t = g.Key, c = g.Count() }).OrderByDescending(g => g.c).Select(g => g.t).FirstOrDefault();
			int totalEvents = await Db.RaceResults.Where(rr => rr.DriverId == driverId).CountAsync();
			int seasonEvents = await Db.RaceResults.Where(rr => rr.DriverId == driverId && rr.Race.SeasonId == seasonId).CountAsync();
			RaceResult bestOverallResult = await Db.RaceResults.Where(rr => rr.DriverId == driverId).OrderBy(rr => rr.Position).Include(rr => rr.Race).FirstOrDefaultAsync();
			RaceResult bestSeasonResult = await Db.RaceResults.Where(rr => rr.DriverId == driverId && rr.Race.SeasonId == seasonId).Include(rr => rr.Race).OrderBy(rr => rr.Position).FirstOrDefaultAsync();
			int seasonPoints = await Db.RaceResults.Where(rr => rr.DriverId == driverId && rr.Race.SeasonId == seasonId).SumAsync(rr => rr.Points);

			return new JsonResult(new SPAData() {
				View = "drivers/detail",
				Data = new {
					season = new DTOSeason(s) {},
					profile = new DTODriverProfile(p) {
						Driver = new DTODriver(p.Driver)
					},
					mainTeam = new DTOTeam(await Db.Teams.FindAsync(mainTeamId)),
					totalEvents = totalEvents,
					seasonEvents = seasonEvents,
					bestOverall = new DTORaceResult(bestOverallResult) {
						Race = new DTORace(bestOverallResult.Race)
					},
					bestSeason = new DTORaceResult(bestSeasonResult) {
						Race = new DTORace(bestSeasonResult.Race)
					},
					seasonPoints = seasonPoints
				}
			});
		}
	}
}