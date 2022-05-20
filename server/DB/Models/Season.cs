using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.DB.Models {
	public class Season {
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public int LastSeasonId { get; set; }
		[JsonIgnore]
		public Season Last { get; set; }
		[Required]
		public int NextSeasonId { get; set; }
		public Season Next { get; set; }
		[Required]
		public int CategoryId { get; set; }
		[JsonIgnore]
		public Category Category { get; set; }
		public List<Race> Races { get; set; }
		public List<Team> Teams { get; set; }
		public Dictionary<String, String> Options { get; set; }
	}
}