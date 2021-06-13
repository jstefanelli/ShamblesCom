using System;

namespace ShamblesCom.Server.DB.Models {
	public class Race {
		public int Id { get; set; }
		public Guid Uuid { get; set; }
		public DateTime DateTime { get; set; }
		public Season Season { get; set; }
		public Track Track { get; set; }
		public string Identifier { get; set; }
		public string? FullName { get; set; }
	}
}