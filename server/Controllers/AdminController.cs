using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.Controllers.Api;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.DTO;
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
				seasons =  await db.Seasons.Select(s => new DTOSeason(s)).ToArrayAsync(),
				categories = await db.Categories.Select(c => new DTOCategory(c)).ToArrayAsync(),
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
					season = await GetSeason(Db, seasonId)
				},
				Redraw = true,
				Url=$"/admin/{seasonId}"
			});
		}

		public static async Task<DTOSeason> GetSeason(ShamblesDBContext Db, int seasonId) {
			Season s = await Db.Seasons.Include(s => s.Races.OrderBy(r => r.DateTime)).Include(s => s.Teams).Where(s => s.Id == seasonId)
				.Include(s => s.Category).ThenInclude(c => c.Game).FirstOrDefaultAsync();

			return new DTOSeason(s) {
				Category = new DTOCategory(s.Category) {
					Game = new DTOGame(s.Category.Game)
				},
				Races = s.Races.Select(r => new DTORace(r)).ToList(),
				Teams = s.Teams.Select(t => new DTOTeam(t)).ToList()
			};
		}
	}		
}