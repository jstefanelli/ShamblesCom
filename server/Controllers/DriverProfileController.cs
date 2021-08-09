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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.Controllers {
	
	[Route("/admin/{seasonId}/drivers/{driverId}")]
	public class DriverProfileController : Controller {

		public static readonly string[] SupportedImageTypes = new string[] {
			"image/png",
			"image/jpeg",
			"image/jpg"
		};
		public ShamblesDBContext Db { get; protected set; }
		public DriverProfileController(ShamblesDBContext db) {
			Db = db;
		}

		private async Task<DriverProfile> GetProfile(int seasonId, int driverId, bool includeDriver = true) {
			DriverProfile profile = includeDriver ? await Db.Profiles.Where(p => p.DriverId == driverId && p.SeasonId == seasonId).Include(p => p.Driver).FirstOrDefaultAsync()
				: await Db.Profiles.Where(p => p.DriverId == driverId && p.SeasonId == seasonId).FirstOrDefaultAsync();
			if (profile == null) {
				profile = new DriverProfile();
				profile.DriverId = driverId;
				profile.SeasonId = seasonId;
				profile.ShortDescription = "A description";
				profile.Description = "A longer description";
				profile.Driver = await Db.Drivers.FindAsync(driverId);

				Db.Profiles.Add(profile);
				await Db.SaveChangesAsync();
			}

			return profile;
		}

		[HttpGet]
		[SPA]
		[Authorize]
		public async Task<ActionResult> Index(int seasonId, int driverId) {
			DriverProfile profile = await GetProfile(seasonId, driverId);

			return new JsonResult(new SPAData {
				View = "admin/driverDetail",
				Redraw = true,
				Url = $"/admin/{seasonId}/drivers/{driverId}",
				Data = new {
					Index = await AdminController.IndexData(Db, seasonId),
					Profile = new DTODriverProfile(profile) {
						Driver = new DTODriver(profile.Driver)
					}
				}
			});
		}

		[HttpPost("pic")]
		[SPA]
		[Authorize]
		public async Task<ActionResult> UploadImage(int seasonId, int driverId, [FromBody] ImageSubmitData file) {
			if (file.Base64Data == null) {
				ModelState.AddModelError("File", "No file provided");
				return new BadRequestObjectResult(ModelState);
			}

			int idx = file.Base64Data.IndexOf(";base64,");
			int dataLength = "data:".Length;
			string mimeType = file.Base64Data.Substring(dataLength, idx - dataLength);
			string dataStr = file.Base64Data.Substring(idx + ";base64,".Length);

			if (!SupportedImageTypes.Contains(mimeType)) {
				ModelState.AddModelError("File", "Unsupported data type");
				return new BadRequestObjectResult(ModelState);
			}

			byte[] data = System.Convert.FromBase64String(dataStr);

			DriverProfile profile = await GetProfile(seasonId, driverId, false);
			profile.ImageData = data;
			profile.ImageMimeType = mimeType;
			Db.Profiles.Update(profile);
			await Db.SaveChangesAsync();

			return new SeeOtherResult($"/admin/{seasonId}/drivers/{driverId}");
		}

		[HttpPost]
		[SPA]
		[Authorize]
		public async Task<ActionResult> Edit(int seasonId, int driverId, [FromBody] DTODriverProfile profileData) {
			DriverProfile profile = await GetProfile(seasonId, driverId);
			profile.ShortDescription = profileData.ShortDescription;
			profile.Description = profileData.Description;

			Db.Profiles.Update(profile);
			await Db.SaveChangesAsync();

			return new SeeOtherResult($"/admin/{seasonId}/drivers/{driverId}");
		}

		public struct ImageSubmitData {
			[JsonPropertyName("base64data")]
			public string Base64Data { get; set; }
		}
	}
}