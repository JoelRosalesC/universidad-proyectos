using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Validations
{
    public class ExpirationDateAttribute : ValidationAttribute
    {
        private const string _dateFormat = "MM/yy";
        private static readonly Regex _regex = new Regex(@"^(0[1-9]|1[0-2])\/\d{2}$");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not string expirationStr)
            {
                return new ValidationResult("El valor debe ser un string.");
            }

            if (!_regex.IsMatch(expirationStr))
            {
                return new ValidationResult("El formato debe ser MM/yy.");
            }

            // Intenta parsear la fecha
            if (!DateTime.TryParseExact(
                    expirationStr,
                    _dateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime expirationDate))
            {
                return new ValidationResult("Fecha inválida.");
            }

            // Ajustar al último día del mes de expiración
            var lastDayOfExpiration = new DateTime(expirationDate.Year, expirationDate.Month, 1).AddMonths(1).AddDays(-1);

            if (DateTime.Today > lastDayOfExpiration)
            {
                return new ValidationResult("Fecha de expiración inválida.");
            }

            return ValidationResult.Success;
        }
    }
}