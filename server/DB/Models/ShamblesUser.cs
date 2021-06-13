using System.ComponentModel.DataAnnotations;

namespace ShamblesCom.Server.DB.Models {
	public class ShamblesUser {
		[Key]
		public int Id { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		public string HashedPassword { get; set; }
	}
}