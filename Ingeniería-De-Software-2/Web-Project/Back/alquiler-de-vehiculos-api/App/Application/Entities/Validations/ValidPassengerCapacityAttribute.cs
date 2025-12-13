

using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Validations
{
    public class ValidPassengerCapacityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue && int.TryParse(stringValue, out var parsed))
            {
                if (parsed >= 1 && parsed <= 100)
                    return ValidationResult.Success;
            }
            
            return new ValidationResult("Passenger capacity must be a number between 1 and 100.");
        }
    }
}