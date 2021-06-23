using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.Models;
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
					index = new {
						seasons = Db.Seasons.ToArray(),
						categories = Db.Categories.ToArray()
					}
				},
				Redraw = true,
				Url = "/admin"
			});
		}

		[HttpGet("{seasonId}")]
		[SPA]
		[Authorize]
		public async Task<ActionResult> WithSeason(int seasonId) {

			return new JsonResult(new SPAData() {
				View = "admin/season",
				Data = new {
					index = new {
						seasons = Db.Seasons.ToArray(),
						categories = Db.Categories.ToArray(),
						selectedSeasonId = seasonId,
					},
					season = await Db.Seasons.Include(s => s.Teams).Where(s => s.Id == seasonId).FirstOrDefaultAsync()
				},
				Redraw = true,
				Url=$"/admin/{seasonId}"
			});
		}

		[HttpGet("{seasonId}/races")]
		[SPA]
		[Authorize]
		public async Task<ActionResult> WithSeasonAndRaces(int seasonId) {
			Season season = await Db.Seasons.Include(s => s.Races).ThenInclude(r => r.RaceResults).Include(s => s.Teams).Where(s => s.Id == seasonId).FirstOrDefaultAsync();

			return new JsonResult(new SPAData() {
				View = "admin/races",
				Data = new {
					index = new {
						seasons = Db.Seasons.ToArray(),
						categories = Db.Categories.ToArray(),
						selectedSeasonId = seasonId,
					},
					games = Db.Games.Include(g => g.Tracks).ToArray(),
					season = season,
					tracks = await Db.Tracks.Where(t => t.GameId == season.Category.Game.Id).ToArrayAsync()
				},
				Redraw = true,
				Url=$"/admin/{seasonId}/races"
			});
		}
	}
}