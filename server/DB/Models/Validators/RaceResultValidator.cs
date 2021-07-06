using System.ComponentModel.DataAnnotations;

namespace ShamblesCom.Server.DB.Models.Validators {
	public class RaceResultValidatorAttribute : ValidationAttribute {
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var raceResult = validationContext.ObjectInstance as RaceResult;

			if (raceResult == null)
				return new ValidationResult("Target object is not a RaceResult");

			if (raceResult.Team.SeasonId != raceResult.Race.SeasonId) {
				return new ValidationResult("The race and team are from different seasons");
			}

			return ValidationResult.Success;
		}
	}
}