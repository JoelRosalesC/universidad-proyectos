

using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Validations
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        public int MinimumAge { get; }

        public MinimumAgeAttribute(int minimumAge)
        {
            MinimumAge = minimumAge;
            ErrorMessage = $"Debes tener al menos {minimumAge} años.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime birthDate)
            {
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;

                if (birthDate > today.AddYears(-age))
                    age--;

                if (age < MinimumAge)
                    return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}