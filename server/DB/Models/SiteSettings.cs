using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShamblesCom.Server.DB.Models {
	public class SiteSettings {
		public int Id { get; set; }
		public int? LiveRaceId { get; set; }
		public Race LiveRace { get; set; }

		public string LiveStreamLinks { get; set; }

		[NotMapped]
		public string[] LiveStreamLinksArray {
			get {
				return LiveStreamLinks?.Split(";") ?? new string[0];
			}

			set {
				StringBuilder builder = new StringBuilder();

				if (value != null && value.Length > 0) {
					for (int i = 0; i < value.Length - 1; i++) {
						builder.Append($"{value[i]};");
					}
					builder.Append(value[value.Length - 1]);
				}

				LiveStreamLinks = builder.ToString();
			}
		}

		public int? CurrentHomeSeasonId { get; set; }
		public Season CurrentHomeSeason { get; set;}
	}
}