using System;
using System.Linq;
using System.Threading.Tasks;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.Seeders {
	public static class GameSeeder {
		public static readonly Game F12020 = new Game() {
			Name = "F1 2020",
			Description = "Formula 1 official game for the 2020 season",
		};

		public static readonly Game ProjectCars2 = new Game() {
			Name = "Project Cars 2",
			Description = "Cross-discipline racig simulator"
		};

		public static readonly Category F12020_F1 = new Category() {
			Name = "Formula 1 (2020)",
			Game = F12020
		};

		public static readonly Category F12020_F2 = new Category() {
			Name = "Formula 2 (2020)",
			Game = F12020
		};

		public static async Task SeedF12020(ShamblesDBContext db) {
			if (!db.Games.Any(g => g.Name == F12020.Name)) {
				Console.WriteLine($"[GameSeeder] Seeding F1 2020");
				await db.Games.AddAsync(F12020);
				await db.SaveChangesAsync();
			}

			if (!db.Categories.Any(c => c.Name == F12020_F1.Name)) {
				F12020_F1.Game = db.Games.First(g => g.Name == F12020.Name);
				await db.Categories.AddAsync(F12020_F1);
				await db.SaveChangesAsync();
			}

			if (!db.Categories.Any(c => c.Name == F12020_F2.Name)) {
				F12020_F2.Game = db.Games.First(g => g.Name == F12020.Name);
				await db.Categories.AddAsync(F12020_F2);
				await db.SaveChangesAsync();
			}
		}

		public static async Task SeedProjectCars2(ShamblesDBContext db) {
			if (!db.Games.Any(g => g.Name == ProjectCars2.Name)) {
				Console.WriteLine($"[GameSeeder] Seeding Project Cars 2");
				await db.Games.AddAsync(ProjectCars2);
				await db.SaveChangesAsync();
			}
		}
	}
}