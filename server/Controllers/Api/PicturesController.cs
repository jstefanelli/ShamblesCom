using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.Controllers.Api
{
    [Route("api/pics")]
    [ApiController]
    [ValidateModel]
    public class PicturesController : ControllerBase
    {
		private ShamblesDBContext Db { get; set; }
		public PicturesController(ShamblesDBContext db) {
			Db = db;
		}

		[HttpGet("profile/{profileId}")]
		public async Task<ActionResult> ProfilePicture(int profileId) {
			DriverProfile rp = await Db.Profiles.FindAsync(profileId);
			if (rp == null) {
				return NotFound();
			}

			if (rp.ImageData == null) {
				return NotFound();
			}

			return File(rp.ImageData, rp.ImageMimeType);
		}
	}
}