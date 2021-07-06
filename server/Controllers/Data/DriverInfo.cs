using System.Collections.Generic;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.Controllers.Data {
	public class DriverInfo {
		public Driver Driver { get; set; }
		public int SeasonPoints { get; set; }
		public int SeasonPosition { get; set; }
		public List<Team> SeasonTeams { get; set; }
	}
}