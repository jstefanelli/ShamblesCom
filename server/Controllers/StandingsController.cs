using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.DTO;
using ShamblesCom.Server.DB.Models;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	[Route("/standings")]
	public class StandingsController : Controller {
		
		private ShamblesDBContext Db { get; set; }

		public StandingsController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet]
		[SPA]
		public async Task<ActionResult> Index() {
			SiteSettings settings = await Db.GetSettings();
			
			Season s = await Db.Seasons.FindAsync(settings.CurrentHomeSeasonId);

			DTOSeason season = null;
			List<DTODriverInfo> drivers = null;
			if (s != null) {
				season = new DTOSeason(s);
				drivers = await Db.Drivers.Where(d => d.RaceResults.Where(rr => rr.Race.SeasonId == s.Id).Count() > 0).Include(d => d.RaceResults)
					.Select(d => new DTODriverInfo(
							new DTODriver(d) {
								RaceResults = d.RaceResults.Where(rr => rr.Race.SeasonId == s.Id).Select(rr => new DTORaceResult(rr)).ToList()
							}
						){
							SeasonPoints = d.RaceResults.Where(rr => rr.Race.SeasonId == s.Id).Sum(rr => rr.Points),
							Profile = new DTODriverProfile(Db.Profiles.Where(dp => dp.DriverId == d.Id && dp.SeasonId == s.Id).FirstOrDefault())
						}
					).ToListAsync();


				drivers.Sort((first, second) => {
					return DTODriverInfo.Compare(first, second);
				});

				for (int i = 0; i < drivers.Count; i++) {
					drivers[i].SeasonPosition = i + 1;
				}
			}


			return new JsonResult(new SPAData() {
				View = "standings/index",
				Data = new {
					Season = season,
					Drivers = drivers
				},
				Redraw = true,
				Url = "/standings"
			});
		}
	}
}