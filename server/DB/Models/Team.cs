using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.DB.Models {
	public class Team {
		[Key]
		public int Id { get; set;}
		public string Name { get; set; }
		public string MainColor { get; set; }
		public string SecondaryColor { get; set; }
		public int SeasonId { get; set; }
		[JsonIgnore]
		public Season Season { get; set; }

		public List<RaceResult> RaceResults { get; set; }
	}
}