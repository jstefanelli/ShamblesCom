using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTOSeason {
		public int Id { get; set; }
		public string Name { get; set; }
		public int LastSeasonId { get; set; }
		public DTOSeason Last { get; set; }
		public int NextSeasonId { get; set; }
		public DTOSeason Next { get; set; }
		public int CategoryId { get; set; }
		public DTOCategory Category { get; set; }
		public List<DTORace> Races { get; set; }
		public List<DTOTeam> Teams { get; set; }

		public DTOSeason(Season s) {
			Id = s.Id;
			Name = s.Name;
			LastSeasonId = s.LastSeasonId;
			NextSeasonId = s.NextSeasonId;
			CategoryId = s.CategoryId;
		}
	}
}
