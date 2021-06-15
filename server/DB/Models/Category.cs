using System;

namespace ShamblesCom.Server.DB.Models {
	public class Category {
		public int Id { get; set; }
		public Guid UUid { get; set; }
		public string Name { get; set; }
		public int GameId { get; set; }
		public Game Game { get; set; }
	}
}