using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTODriver {
		public int Id { get; set; }
		public string Nickname { get; set; }
		public int Number { get; set; }
		public List<DTORaceResult> RaceResults { get; set; }
		public List<DTODriverProfile> Profiles { get; set; }

		public DTODriver(Driver d) {
			Id = d.Id;
			Nickname = d.Nickname;
			Number = d.Number;
		}
	}
}