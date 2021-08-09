using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTODriverProfile {
		public int Id { get; set; }
		public int DriverId { get; set; }
		public DTODriver Driver { get; set; }
		public int SeasonId { get; set; }
		public DTOSeason Season { get; set; }
		public string ShortDescription { get; set; }
		public string Description { get; set; }
		public string ImageLink { get; set; }

		public DTODriverProfile() {
			
		}

		public DTODriverProfile(DriverProfile pr) {
			if (pr == null)
				return;

			Id = pr.Id;
			DriverId = pr.DriverId;
			ShortDescription = pr.ShortDescription;
			SeasonId = pr.SeasonId;
			Description = pr.Description;
			ImageLink = $"/api/pics/profile/{Id}";
		}
	}
}