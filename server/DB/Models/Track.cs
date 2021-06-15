using System;
using System.Collections.Generic;

namespace ShamblesCom.Server.DB.Models {
	public class Track {
		public int Id { get; set; }
		public Guid Uuid { get; set; }
		public int GameId { get; set; }
		public Game Game { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }

		public List<Race> Races { get; set; }
	}
}