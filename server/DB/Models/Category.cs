using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.DB.Models {
	public class Category {
		public int Id { get; set; }
		public string Name { get; set; }
		public int GameId { get; set; }
		[JsonIgnore]
		public Game Game { get; set; }

		public List<Season> Seasons { get; set; }
	}
}