using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.DTO;
using ShamblesCom.Server.DB.Models;
using ShamblesCom.Server.SPA;

namespace ShamblesCom.Server.Controllers {
	[Route("/race")]
	public class RaceViewController : Controller {
		private ShamblesDBContext Db { get; set; }

		public RaceViewController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet("{id}")]
		[SPA]
		public async Task<ActionResult> Index(int id) {
			Race r = await Db.Races.FindAsync(id);
			if (r == null) {
				return new SeeOtherResult("/homepage");
			}

			DTORace race = new DTORace(r) {
				RaceResults = await Db.RaceResults.Where(rr => rr.RaceId == r.Id).Include(rr => rr.Driver).ThenInclude(d => d.Profiles.Where(p => p.SeasonId == r.SeasonId))
								.Include(rr => rr.Team)
								.Select(rr => new DTORaceResult(rr) {
									Team = new DTOTeam(rr.Team),
									Driver = new DTODriver(rr.Driver) {
										Profiles = rr.Driver.Profiles.Select(p => new DTODriverProfile(p)).ToList()
									}
								}).ToListAsync()
			};

			return new JsonResult(new SPAData() {
				View = "race/index",
				Data = new {
					Race = race
				},
				Redraw = false,
				Url = $"/race/{id}"
			});
		}
	}
}