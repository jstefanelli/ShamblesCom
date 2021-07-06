using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShamblesCom.Server.DB.Models {
	public class Driver {
		[Key]
		public int Id { get; set; }
		public string Nickname { get; set; }
		public int Number { get; set; }
		public List<RaceResult> RaceResults { get; set; }

	}
}