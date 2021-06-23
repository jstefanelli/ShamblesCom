using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.DB.Models {
	public class Race {
		public int Id { get; set; }
		public DateTime DateTime { get; set; }
		public int SeasonId { get; set; }
		[JsonIgnore]
		public Season Season { get; set; }
		public int Trackid { get; set; }
		[JsonIgnore]
		public Track Track { get; set; }
		public string Identifier { get; set; }
		public string LivestreamLink { get; set; }
		public string VodLink { get; set; }
		public List<RaceResult> RaceResults { get; set;}
	}
}