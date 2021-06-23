using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models.Validators;

namespace ShamblesCom.Server.DB.Models {
	[RaceResultValidator]
	public class RaceResult {
		[Required]
		public int Id { get; set; }
		[Required]
		public int DriverId { get; set; }
		public Driver Driver { get; set; }
		[Required]
		public int TeamId { get; set; }
		public Team Team { get; set; }
		[Required]
		public int RaceId { get; set; }
		[JsonIgnore]
		public Race Race { get; set; }
		[Required]
		[Range(0, int.MaxValue)]
		public int Position { get; set; }
		public bool Finished { get; set; } = true;
		[Range(0, int.MaxValue)]
		public int Penalties { get; set; } = 0;
		[Required]
		public int StartPosition { get; set;}
		[Required]
		public bool FastestLap { get; set; }
	}
}