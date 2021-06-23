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

				Track[] tracks = new Track[] {
					new Track {
						Name = "Melbourne Grand Prix Circuit",
						Country = "Australia",
						GameId = F12020.Id
					},
					new Track {
						Name = "Bahrain International Circuit",
						Country = "Bahrain",
						GameId = F12020.Id
					},
					new Track {
						Name = "Bahrain International Circuit (Short version)",
						Country = "Bahrain",
						GameId = F12020.Id
					},
					new Track {
						Name = "Hanoi Circuit",
						Country = "Vietnam",
						GameId = F12020.Id
					},
					new Track {
						Name = "Circuit Zandvoort",
						Country = "Netherlands",
						GameId = F12020.Id
					},
					new Track {
						Name = "Shanghai International Circuit",
						Country = "China",
						GameId = F12020.Id
					},
					new Track {
						Name = "Circuit de Barcelona-Catalunya",
						Country = "Spain",
						GameId = F12020.Id
					},
					new Track {
						Name = "Circuit de Monaco",
						Country = "Monaco",
						GameId = F12020.Id
					},
					new Track {
						Name = "Baku City Circuit",
						Country = "Azerbaijan",
						GameId = F12020.Id
					},
					new Track {
						Name = "Circuit Gilles-Villeneuve",
						Country = "Canada",
						GameId = F12020.Id
					},
					new Track {
						Name = "Circuit Paul Ricard",
						Country = "France",
						GameId = F12020.Id
					},
					new Track {
						Name = "Spielberg - Red Bull Ring",
						Country = "Austria",
						GameId = F12020.Id
					},
					new Track {
						Name = "Silverstone Circuit",
						Country = "Great Britain",
						GameId = F12020.Id
					},
					new Track {
						Name = "Silverstone Circuit (Short version)",
						Country = "Great Britain",
						GameId = F12020.Id
					},
					new Track {
						Name = "Hungaroring",
						Country = "Hungary",
						GameId = F12020.Id
					},
					new Track {
						Name = "Circuit de Spa-Francorchamps",
						Country = "Belgium",
						GameId = F12020.Id
					},
					new Track {
						Name = "Autodromo Nazionale di Monza",
						Country = "Italy",
						GameId = F12020.Id
					},
					new Track {
						Name = "Marina Bay Street Circuit",
						Country = "Singapore",
						GameId = F12020.Id
					},
					new Track {
						Name = "Sochi Autodrom",
						Country = "Russia",
						GameId = F12020.Id
					},
					new Track {
						Name = "Suzuka International Racing Course",
						Country = "Japan",
						GameId = F12020.Id
					},
					new Track {
						Name = "Suzuka International Racing Course (Short version)",
						Country = "Japan",
						GameId = F12020.Id
					},
					new Track {
						Name = "Circuit of the Americas",
						Country = "USA",
						GameId = F12020.Id
					},
					new Track {
						Name = "Circuit of the Americas (Short version)",
						Country = "USA",
						GameId = F12020.Id
					},
					new Track {
						Name = "Autódromo Hermanos Rodríguez",
						Country = "México",
						GameId = F12020.Id
					},
					new Track {
						Name = "Autodromo José Carlos Pace",
						Country = "Brazil",
						GameId = F12020.Id
					},
					new Track {
						Name = "Yas Marina Circuit",
						Country = "Abu Dhabi",
						GameId = F12020.Id
					}
				};

				await db.Tracks.AddRangeAsync(tracks);
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