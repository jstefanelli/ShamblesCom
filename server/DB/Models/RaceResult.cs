namespace ShamblesCom.Server.DB.Models {
	public class RaceResult {
		public int Id { get; set; }
		public Driver Driver { get; set; }
		public Race Race { get; set; }
		public int Position { get; set; }
		public bool Finished { get; set; }
		public int Penalties { get; set; }
	}
}