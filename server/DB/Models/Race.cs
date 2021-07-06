using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.DB.Models {
	public class Race {
		public int Id { get; set; }
		public DateTime DateTime { get; set; }
		public int SeasonId { get; set; }
		[JsonIgnore]
		public Season Season { get; set; }
		public int TrackId { get; set; }
		[JsonIgnore]
		public Track Track { get; set; }
		[Required]
		[StringLength(int.MaxValue, MinimumLength = 3)]
		public string Name { get; set; }
		public string LivestreamLink { get; set; }
		public string VodLink { get; set; }
		public List<RaceResult> RaceResults { get; set;}
	}
}