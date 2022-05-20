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

			return new JsonResult(new SPAData() {
				View = "drivers/index",
				Data = new {
					profiles = profiles,
					season = await Task.Run(() => { 
						return new DTOSeason(currentSeason) {
							Races = currentSeason.Races.Select(r => new DTORace(r) {
								RaceResults = r.RaceResults.Select(rr => new DTORaceResult(rr)).ToList()
							}).ToList()
						};
					})
				}
			});
		}

		[HttpGet("{driverId:int}")]
		[SPA]
		public async Task<ActionResult> Details(int driverId, [FromQuery] int seasonId = -1) {
			if (seasonId == -1) {
				seasonId = (await Db.Settings.FirstOrDefaultAsync())?.CurrentHomeSeasonId ?? throw new Exception("No default season ID");
			}

			Season s = await Db.Seasons.Include(s => s.Races.Where(r => r.RaceResults.Where(rr => rr.DriverId == driverId).Count() > 0)).ThenInclude(r => r.RaceResults.Where(rr => rr.DriverId == driverId)).FirstOrDefaultAsync();
			
			DriverProfile p = await Db.Profiles.Where(p => p.DriverId == driverId && p.SeasonId == seasonId).FirstOrDefaultAsync();

			return new JsonResult(new SPAData() {
				View = "drivers/detail",
				Data = new {
					season = new DTOSeason(s) {
						Races = s.Races.Select(r => new DTORace(r) {
							RaceResults = r.RaceResults.Select(rr => new DTORaceResult(rr)).ToList()
						}).ToList()
					},
					profile = new DTODriverProfile(p)
				}
			});
		}
	}
}