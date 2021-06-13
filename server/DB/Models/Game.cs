using System;

namespace ShamblesCom.Server.DB.Models {
	public class Game {
		public int Id { get; set; }
		public Guid Uuid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}
}