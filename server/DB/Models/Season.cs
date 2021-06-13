using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShamblesCom.Server.DB.Models {
	public class Season {
		public int Id { get; set; }
		public Guid Uuid { get; set; }
		public string Name { get; set; }

		
		public Season? Last { get; set; }
		public Season? Next { get; set; }
		public Category Category { get; set; }
	}
}