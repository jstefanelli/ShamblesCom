using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.Seeders {
	public static class SettingsSeeder {
		public static async Task SeedSettings(ShamblesDBContext db) {
			if (db.Settings.Count() == 0) {
				SiteSettings st = new SiteSettings() {
					CurrentHomeSeasonId = await db.Seasons.Select(s => s.Id).FirstOrDefaultAsync()
				};

				db.Settings.Add(st);
				await db.SaveChangesAsync();
			} else if (db.Settings.Count() > 1) {
				db.Settings.RemoveRange(db.Settings.Where(st => st.Id > 1));
				await db.SaveChangesAsync();
			}
		}
	}

}