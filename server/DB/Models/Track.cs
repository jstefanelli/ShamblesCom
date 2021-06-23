using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.DB.Models {
	public class Track {
		public int Id { get; set; }
		public int GameId { get; set; }
		[JsonIgnore]
		public Game Game { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }

		public List<Race> Races { get; set; }
	}
}