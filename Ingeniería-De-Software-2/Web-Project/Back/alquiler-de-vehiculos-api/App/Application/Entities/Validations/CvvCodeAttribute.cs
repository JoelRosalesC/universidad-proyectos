using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Validations
{
   public class CvvCodeAttribute : ValidationAttribute
    {
        private static readonly Regex _regex = new Regex(@"^\d{3,4}$");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not string cvv)
            {
                return new ValidationResult("El CVV no es valido.");
            }

            if (!_regex.IsMatch(cvv))
            {
                return new ValidationResult("El CVV no es valido.");
            }

            return ValidationResult.Success;
        }
    }
}