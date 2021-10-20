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
		public async Task<ActionResult> Index() {
			SiteSettings settings = await Db.Settings.FirstOrDefaultAsync();

			List<Driver> seasonDrivers = new List<Driver>();

			if (settings == null) {
				return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
			}

			Season currentSeason = await Db.Seasons.Where(s => s.Id == settings.CurrentHomeSeasonId ).FirstOrDefaultAsync();
			List<DTODriverInfo> profiles = await Db.Profiles.Where(p => p.SeasonId == currentSeason.Id)
				.Include(p => p.Driver)
					.ThenInclude(d => d.RaceResults.Where(rr => rr.Race.SeasonId == currentSeason.Id))
						.ThenInclude(rr => rr.Race)
				.Select(p => new DTODriverInfo(new DTODriver(p.Driver) {
					RaceResults = p.Driver.RaceResults.Select(rr => new DTORaceResult(rr)).ToList()
				}) {
					Profile = new DTODriverProfile(p),
					SeasonPoints = p.Driver.RaceResults.Sum(rr => rr.Points),
				})
				.ToListAsync();

			foreach(var i in profiles) {
				int maxTeamId = 0;
				int maxTeamAmount = 0;
				foreach(int teamId in i.Driver.RaceResults.Select(t => t.TeamId).Distinct()) {
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
					season = new DTOSeason(currentSeason)
				}
			});
		} 
	}
}