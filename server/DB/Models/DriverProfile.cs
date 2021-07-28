using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShamblesCom.Server.DB.Models {
	public class DriverProfile {
		[Key]
		public int Id { get; set; }
		public int DriverId { get; set; }
		public Driver Driver { get; set; }
		public int SeasonId { get; set; }
		public Season Season { get; set; }
		public string ShortDescription { get; set; }
		public string Description { get; set; }
		public byte[] ImageData { get; set; }
		public string ImageMimeType { get; set; }
	}
}