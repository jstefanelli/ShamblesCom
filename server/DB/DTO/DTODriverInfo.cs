using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShamblesCom.Server.DB.DTO {
	public class DTODriverInfo {
		public DTODriver Driver { get; set; }
		public int SeasonPoints { get; set; }
		public int SeasonPosition { get; set; }
		public DTODriverProfile Profile { get; set; }

		public DTODriverInfo(DTODriver driver) {
			Driver = driver;
		}
		
		public static int Compare(DTODriverInfo d0, DTODriverInfo d1, int pos = 1) {
			if (d0.SeasonPoints != d1.SeasonPoints) {
				return d1.SeasonPoints - d0.SeasonPoints;
			}

			if (pos >= 50)
				return 0;

			int d0p = d0.Driver.RaceResults.Where(rr => rr.Position == pos && rr.Finished).Count();
			int d1p = d1.Driver.RaceResults.Where(rr => rr.Position == pos && rr.Finished).Count();

			if (d0p - d1p == 0) {
				return Compare(d0, d1, pos + 1);
			}

			return d1p - d0p;
		}
	}

}