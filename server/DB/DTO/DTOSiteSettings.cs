using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTOSiteSettings {
		public int Id { get; set; }
		public int? LiveRaceId { get; set; }
		public Race LiveRace { get; set; }

		public string[] LiveStreamLinks { get; set; }

		public int? CurrentHomeSeasonId { get; set; }
		public Season CurrentHomeSeason { get; set;}

		public DTOSiteSettings(SiteSettings c) {
			Id = c.Id;
			LiveRaceId = c.LiveRaceId;
			CurrentHomeSeasonId = c.CurrentHomeSeasonId;
			LiveStreamLinks = c.LiveStreamLinksArray;
		}
	}
}