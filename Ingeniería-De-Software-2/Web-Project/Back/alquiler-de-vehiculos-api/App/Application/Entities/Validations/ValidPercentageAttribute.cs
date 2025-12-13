

using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Validations
{
    public class ValidPercentageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue && double.TryParse(stringValue, out var parsed))
            {
                if (parsed >= 0 && parsed <= 100)
                    return ValidationResult.Success;
            }

            return new ValidationResult("Return percentage must be a number between 0 and 100.");
        }
    }

}