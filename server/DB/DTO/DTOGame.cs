using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTOGame {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<DTOTrack> Tracks { get; set;}
		public List<DTOCategory> Categories { get; set;}

		public DTOGame(Game g) {
			Id = g.Id;
			Name = g.Name;
			Description = g.Description;
		}
	}
}