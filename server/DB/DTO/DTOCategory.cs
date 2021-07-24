using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTOCategory {
		public int Id { get; set; }
		public string Name { get; set; }
		public int GameId { get; set; }
		public DTOGame Game { get; set; }

		public List<DTOSeason> Seasons { get; set; }

		public DTOCategory(Category c) {
			Id = c.Id;
			Name = c.Name;
			GameId = c.GameId;
		}
	}
}