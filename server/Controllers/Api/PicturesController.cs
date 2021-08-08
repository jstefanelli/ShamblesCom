using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.Assets;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.Controllers.Api
{
    [Route("api/pics")]
    [ApiController]
    [ValidateModel]
    public class PicturesController : ControllerBase
    {
		private const string MissingProfilePictureAssetName = "Missing_profile.png";

		private ShamblesDBContext Db { get; set; }
		private IFrontendAssets Assets { get; set; }
		public PicturesController(ShamblesDBContext db, IFrontendAssets assets) {
			Db = db;
			Assets = assets;
		}

		private ActionResult GetMissingProfilePicture() {
			string asset_path = Assets.GetPublicPath(MissingProfilePictureAssetName);
			if (asset_path == null) {
				return NotFound();
			} else {
				return Redirect(asset_path);
			}
		}

		[HttpGet("profile/{profileId}")]
		public async Task<ActionResult> ProfilePicture(int profileId) {
			DriverProfile rp = await Db.Profiles.FindAsync(profileId);
			if (rp == null) {
				return GetMissingProfilePicture();
			}

			if (rp.ImageData == null || rp.ImageData.Length == 0) {
				return GetMissingProfilePicture();
			}

			return File(rp.ImageData, rp.ImageMimeType);
		}
	}
}