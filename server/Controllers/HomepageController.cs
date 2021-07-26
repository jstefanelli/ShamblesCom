using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ShamblesCom.Server.SPA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using ShamblesCom.Server.DB;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.DB.Models;
using ShamblesCom.Server.DB.DTO;

namespace ShamblesCom.Server.Controllers {
	[Route("/homepage")]
	public class HomepageController : ControllerBase {
		protected ShamblesDBContext Db { get; set; }

		public HomepageController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet]
		[SPA]
		public async Task<ActionResult> Get() {
			SiteSettings settings = await Db.Settings.FirstOrDefaultAsync();

			Race lastRace = null;
			Race nextRace = null;

			if (settings != null) {

				DateTime now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

				Race tmp = Db.Races.Where(r => r.DateTime > now).FirstOrDefault();
			
				lastRace = await Db.Races.Where(r => r.DateTime < now && r.SeasonId == settings.CurrentHomeSeasonId).OrderByDescending(r => r.DateTime)
					.Include(r => r.RaceResults).ThenInclude(rr => rr.Team).Include(r => r.RaceResults).ThenInclude(rr => rr.Driver).FirstOrDefaultAsync();
				nextRace = await Db.Races.Where(r => r.DateTime > now && r.SeasonId == settings.CurrentHomeSeasonId).OrderBy(r => r.DateTime).FirstOrDefaultAsync();
			}

			if (lastRace != null && lastRace.RaceResults != null) {
				lastRace.RaceResults.Sort();
			}

			return new JsonResult(new SPAData() {
				View = "homepage",
				Redraw = false,
				Data = new {
					lastRace = lastRace != null ? new DTORace(lastRace) {
						RaceResults = await Task.Run(() => lastRace.RaceResults?.Select(rr => new DTORaceResult(rr) {
							Driver = new DTODriver(rr.Driver),
							Team = new DTOTeam(rr.Team)
						}).ToList())
					} : null,
					nextRace = nextRace != null ? new DTORace(nextRace) : null				},
				Url = "/homepage"
			});

		}
	}
}