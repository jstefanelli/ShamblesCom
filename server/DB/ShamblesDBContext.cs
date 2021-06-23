using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB {
	public class ShamblesDBContext : DbContext {
		public const string DBFilePath = "shambles.db";

		public DbSet<ShamblesUser> Users { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Driver> Drivers { get; set; }
		public DbSet<Game> Games { get; set; }
		public DbSet<Race> Races { get; set; }
		public DbSet<RaceResult> RaceResults { get; set; }
		public DbSet<Season> Seasons { get; set; }
		public DbSet<Track> Tracks { get; set; }
		public DbSet<Team> Teams { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseSqlite($"Data Source={DBFilePath}");
		}

		protected override void OnModelCreating(ModelBuilder builder) {
			builder.Entity<Season>()
				.HasOne(s => s.Last)
				.WithOne(s => s.Next);

			builder.Entity<ShamblesUser>()
				.HasIndex(u => u.Email)
				.IsUnique();

			builder.Entity<Category>()
				.HasIndex(c => c.Name)
				.IsUnique();

			builder.Entity<Driver>()
				.HasIndex(d => d.Nickname)
				.IsUnique();

			builder.Entity<Game>()
				.HasIndex(d => d.Name)
				.IsUnique();

			builder.Entity<Team>()
				.HasOne(t => t.Season)
				.WithMany(s => s.Teams);

			builder.Entity<Team>()
				.HasIndex(t => new { t.Name, t.SeasonId})
				.IsUnique();

			builder.Entity<Team>()
				.Navigation<Season>(t => t.Season);

			builder.Entity<Race>()
				.HasOne(r => r.Season)
				.WithMany(s => s.Races);

			builder.Entity<Track>()
				.HasIndex(t => new { t.GameId, t.Name })
				.IsUnique();
			
		}
	}
}