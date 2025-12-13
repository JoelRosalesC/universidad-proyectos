using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Validations
{
    public class ValidYearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue && int.TryParse(stringValue, out var parsed))
            {
                if (parsed >= 1900 && parsed <= 2025)
                    return ValidationResult.Success;
            }
            
            return new ValidationResult("El año ingresado es inválido");
        }
    }
}