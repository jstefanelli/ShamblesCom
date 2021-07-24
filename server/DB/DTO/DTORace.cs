using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTORace {
		
		public int Id { get; set; }
		public DateTime DateTime { get; set; }
		public int SeasonId { get; set; }
		public DTOSeason Season { get; set; }
		public int TrackId { get; set; }
		public DTOTrack Track { get; set; }
		public string Name { get; set; }
		public string LivestreamLink { get; set; }
		public string VodLink { get; set; }

		
		public List<DTORaceResult> RaceResults { get; set;}

		public DTORace(Race r) {
			Id = r.Id;
			DateTime = r.DateTime;
			Name = r.Name;
			SeasonId = r.SeasonId;
			TrackId = r.TrackId;
			Name = r.Name;
			LivestreamLink = r.LivestreamLink;
			VodLink = r.VodLink;
		}
	}
}