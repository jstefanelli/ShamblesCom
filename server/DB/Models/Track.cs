using System;

namespace ShamblesCom.Server.DB.Models {
	public class Track {
		public int Id { get; set; }
		public Guid Uuid { get; set; }
		public Game Game { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }
	}
}