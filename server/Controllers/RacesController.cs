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
using ShamblesCom.Server.DB.DTO;

namespace ShamblesCom.Server.Controllers {
	
	[Route("/admin/{seasonId}/[controller]")]
	public class RacesController : Controller {
		protected ShamblesDBContext Db { get; set;}
		public RacesController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet("{raceId?}")]
		[SPA]
		public async Task<ActionResult> WithSeasonAndRaces(int seasonId, int? raceId = null) {
			DTOSeason season = await AdminController.GetSeason(Db, seasonId);

			List<DTORaceResult> rrs = raceId.HasValue ? await Db.RaceResults.Where(rr => rr.RaceId == raceId).Include(rr => rr.Driver).Include(rr => rr.Team).Select(rr => new DTORaceResult(rr) {
				Driver = new DTODriver(rr.Driver),
				Team = new DTOTeam(rr.Team),
			}).ToListAsync() : null;

			object index = await AdminController.IndexData(Db, seasonId);
			DTOGame[] games = await Db.Games.Include(g => g.Tracks).Select(g => new DTOGame(g)).ToArrayAsync();
			DTOTrack[] tracks = await Db.Tracks.Where(t => t.GameId == season.Category.Game.Id).Select(tr => new DTOTrack(tr)).ToArrayAsync();
			DTODriver[] drivers = await Db.Drivers.Select(d => new DTODriver(d)).ToArrayAsync();

			return new JsonResult(new SPAData() {
				View = "admin/races",
				Data = new {
					index = index,
					games = games,
					season = season,
					selectedRaceId = raceId,
					tracks = tracks,
					results = rrs,
					drivers = drivers
				},
				Redraw = true,
				Url= raceId == null ? $"/admin/{seasonId}/races/" : $"/admin/{seasonId}/races/{raceId}"
			});
		}

		[HttpPost("")]
		[SPA]
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

		[HttpPut("{raceId}")]
		[SPA]
		[ValidateModel]
		public async Task<ActionResult> EditRace(int seasonId, int raceId, [FromBody] Race race) {
			if (race.Id != raceId) {
				ModelState.AddModelError("Id", "The URI's RaceId does not match the content URI");
			}

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


		[HttpPost("{raceId}/results")]
		[SPA]
		[ValidateModel]
		public async Task<ActionResult> AddResult(int seasonId, int raceId, [FromBody] RaceResult result) {
			Race r = await Db.Races.FindAsync(raceId);
			if (r == null) {
				ModelState.AddModelError("RaceId", "Race not found");
			}

			Team t = await Db.Teams.Where(t => t.Id == result.TeamId && t.SeasonId == seasonId).FirstOrDefaultAsync();
			if (t == null) {
				ModelState.AddModelError("TeamId", "Team not found");
			}

			Driver d = await Db.Drivers.FindAsync(result.DriverId);
			if (d == null) {
				ModelState.AddModelError("DriverId", "Driver not found");
			}

			if (await Db.RaceResults.Where(rr => rr.RaceId == raceId && rr.DriverId == result.DriverId).AnyAsync()) {
				ModelState.AddModelError("DriverId", "A result for this driver is already present");
			}

			if (!ModelState.IsValid) {
				return new BadRequestObjectResult(ModelState);
			}

			Db.RaceResults.Add(result);
			await Db.SaveChangesAsync();

			return new SeeOtherResult($"/admin/{seasonId}/races/{raceId}");
		}

		[HttpPut("{raceId}/results/{resultId}")]
		[SPA]
		[ValidateModel]
		public async Task<ActionResult> EditResult(int seasonId, int raceId, int resultId, [FromBody] RaceResult result) {
			Race r = await Db.Races.FindAsync(raceId);
			if (r == null) {
				ModelState.AddModelError("RaceId", "Race not found");
			}

			Team t = await Db.Teams.Where(t => t.Id == result.TeamId && t.SeasonId == seasonId).FirstOrDefaultAsync();
			if (t == null) {
				ModelState.AddModelError("TeamId", "Tream not found");
			}

			Driver d = await Db.Drivers.FindAsync(result.DriverId);
			if (d == null) {
				ModelState.AddModelError("DriverId", "Driver not found");
			}
			
			if (await Db.RaceResults.Where(rr => rr.RaceId == raceId && rr.DriverId == result.DriverId && rr.Id != resultId).AnyAsync()) {
				ModelState.AddModelError("DriverId", "A result for this driver is already present");
			}

			Db.RaceResults.Update(result);
			await Db.SaveChangesAsync();

			return new SeeOtherResult($"/admin/{seasonId}/races/{raceId}");
		}
	}
}
