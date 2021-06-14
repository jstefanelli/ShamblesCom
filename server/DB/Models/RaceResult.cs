using System.ComponentModel.DataAnnotations;
using ShamblesCom.Server.DB.Models.Validators;

namespace ShamblesCom.Server.DB.Models {
	[RaceResultValidator]
	public class RaceResult {
		[Required]
		public int Id { get; set; }
		[Required]
		public Driver Driver { get; set; }
		[Required]
		public Team Team { get; set; }
		[Required]
		public Race Race { get; set; }
		[Required]
		[Range(0, int.MaxValue)]
		public int Position { get; set; }
		public bool Finished { get; set; } = true;
		[Range(0, int.MaxValue)]
		public int Penalties { get; set; } = 0;
	}
}