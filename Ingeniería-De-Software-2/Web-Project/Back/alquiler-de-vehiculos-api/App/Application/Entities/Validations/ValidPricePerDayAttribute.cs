

using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Validations
{
    public class ValidPricePerDayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue && double.TryParse(stringValue, out var parsed))
            {
                if (parsed > 0)
                    return ValidationResult.Success;
            }

            return new ValidationResult("Price per day must be a valid number greater than 0.");
        }
    }
}