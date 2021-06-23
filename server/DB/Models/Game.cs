using System;
using System.Collections.Generic;

namespace ShamblesCom.Server.DB.Models {
	public class Game {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<Track> Tracks { get; set;}
		public List<Category> Categories { get; set;}
	}
}