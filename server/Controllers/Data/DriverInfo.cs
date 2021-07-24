using System.Collections.Generic;
using ShamblesCom.Server.DB.Models;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.DTO;

namespace ShamblesCom.Server.Controllers.Data {
	public class DriverInfo {
		[JsonPropertyName("driver")]
		public DTODriver Driver { get; set; }
		[JsonPropertyName("seasonPoints")]
		public int SeasonPoints { get; set; }
		[JsonPropertyName("seasonPosition")]
		public int SeasonPosition { get; set; }
	}
}