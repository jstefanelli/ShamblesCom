using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTOTeam {
		public int Id { get; set;}
		public string Name { get; set; }
		public string MainColor { get; set; }
		public string SecondaryColor { get; set; }
		public int SeasonId { get; set; }
		public Season Season { get; set; }

		public DTOTeam(Team t) {
			Id = t.Id;
			Name = t.Name;
			MainColor = t.MainColor;
			SecondaryColor = t.SecondaryColor;
			SeasonId = t.SeasonId;
		}
	}
}