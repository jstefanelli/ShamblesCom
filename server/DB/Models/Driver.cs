using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShamblesCom.Server.DB.Models {
	public class Driver {
		[Key]
		public int Id { get; set; }
		[StringLength(512, MinimumLength = 1)]
		public string Nickname { get; set; }
		public int Number { get; set; }

		public List<RaceResult> RaceResults { get; set; }
		public List<DriverProfile> Profiles { get; set; }
	}
}