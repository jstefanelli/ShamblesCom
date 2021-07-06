using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.Controllers.Api;
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

		public static async Task<Object> IndexData(ShamblesDBContext db, int selectedSeasonId = 0) {
			return new {
				seasons = await db.Seasons.ToArrayAsync(),
				categories = await db.Categories.ToArrayAsync(),
				selectedSeasonId = selectedSeasonId
			};
		}

		[HttpGet]
		[SPA]
		[Authorize]
		public async Task<ActionResult> Index() {
			return new JsonResult(new SPAData() {
				View = "admin/home",
				Data = new {
					index = await IndexData(Db)
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
					index = await IndexData(Db, seasonId),
					season = await GetSeason(seasonId)
				},
				Redraw = true,
				Url=$"/admin/{seasonId}"
			});
		}

		private Task<Season> GetSeason(int seasonId) {
			return Db.Seasons.Include(s => s.Races.OrderBy(r => r.DateTime)).ThenInclude(r => r.RaceResults.OrderBy(rr => rr.Position)).Include(s => s.Teams).Where(s => s.Id == seasonId).FirstOrDefaultAsync();
		}

		[HttpGet("{seasonId}/races/{raceId?}")]
		[SPA]
		[Authorize]
		public async Task<ActionResult> WithSeasonAndRaces(int seasonId, int? raceId = null) {
			Season season = await GetSeason(seasonId);

			return new JsonResult(new SPAData() {
				View = "admin/races",
				Data = new {
					index = await IndexData(Db, seasonId),
					games = Db.Games.Include(g => g.Tracks).ToArray(),
					season = season,
					selectedRaceId = raceId,
					tracks = await Db.Tracks.Where(t => t.GameId == season.Category.Game.Id).ToArrayAsync()
				},
				Redraw = true,
				Url= raceId == null ? $"/admin/{seasonId}/races/" : $"/admin/{seasonId}/races/{raceId}"
			});
		}

		[HttpPost("{seasonId}/races")]
		[SPA]
		[Authorize]
		[ValidateModel]
		public async Task<ActionResult> AddRace(int seasonId, [FromBody] Race race) {
			Track track = await Db.Tracks.FindAsync(race.TrackId);
            if (track == null) {
                ModelState.AddModelError("TrackId", "The given track was not found");
            }
            Season season = await Db.Seasons.FindAsync(race.SeasonId);
            if (season == null) {
                ModelState.AddModelError("SeasonId", "The given season was not found");
            }

            if (!ModelState.IsValid) {
                return new BadRequestObjectResult(ModelState);
            }

            race.DateTime = race.DateTime.ToUniversalTime();

            Db.Races.Add(race);
            await Db.SaveChangesAsync();

			return new SeeOtherResult($"/admin/{seasonId}/races/{race.Id}");
		}

		[HttpPut("{seasonId}/races/{raceId}")]
		[SPA]
		[Authorize]
		[ValidateModel]
		public async Task<ActionResult> EditRace(int seasonId, int raceId, [FromBody] Race race) {
			Track track = await Db.Tracks.FindAsync(race.TrackId);
            if (track == null) {
                ModelState.AddModelError("TrackId", "The given track was not found");
            }
            Season season = await Db.Seasons.FindAsync(race.SeasonId);
            if (season == null) {
                ModelState.AddModelError("SeasonId", "The given season was not found");
            }

            if (!ModelState.IsValid) {
                return new BadRequestObjectResult(ModelState);
            }

            race.DateTime = race.DateTime.ToUniversalTime();
			race.Id = raceId;

            Db.Races.Update(race);
            await Db.SaveChangesAsync();

			return new SeeOtherResult($"/admin/{seasonId}/races/{raceId}");
		}
	}
}