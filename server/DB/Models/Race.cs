using System;
using System.Collections.Generic;

namespace ShamblesCom.Server.DB.Models {
	public class Race {
		public int Id { get; set; }
		public Guid Uuid { get; set; }
		public DateTime DateTime { get; set; }
		public int SeasonId { get; set; }
		public Season Season { get; set; }
		public int Trackid { get; set; }
		public Track Track { get; set; }
		public string Identifier { get; set; }
		public string? FullName { get; set; }
		public List<RaceResult> RaceResults { get; set;}
	}
}