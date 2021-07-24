using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTOTrack {
		public int Id { get; set; }
		public int GameId { get; set; }
		public Game Game { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }

		public DTOTrack(Track tr) {
			Id = tr.Id;
			GameId = tr.GameId;
			Name = tr.Name;
			Country = tr.Country;
		}
	}
}