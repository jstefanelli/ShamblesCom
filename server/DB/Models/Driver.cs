using System;
using System.ComponentModel.DataAnnotations;

namespace ShamblesCom.Server.DB.Models {
	public class Driver {
		[Key]
		public int Id { get; set; }
		public Guid Uuid { get; set; }
		public string Nickname { get; set; }

	}
}