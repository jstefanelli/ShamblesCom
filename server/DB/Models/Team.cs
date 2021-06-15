using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShamblesCom.Server.DB.Models {
	public class Team {
		[Key]
		public int Id { get; set;}
		public Guid Uuid { get; set; }
		public string Name { get; set; }
		public string MainColor { get; set; }
		public string SecondaryColor { get; set; }
		public int SeasonId { get; set; }
		public Season Season { get; set; }

		public List<RaceResult> RaceResults { get; set; }
	}
}