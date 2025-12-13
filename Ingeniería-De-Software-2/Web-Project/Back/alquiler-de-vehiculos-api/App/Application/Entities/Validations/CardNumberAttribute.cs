using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AlquilerDeVehiculosApi.App.Application.Entities.Validations
{
   public class CardNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not string cardNumber)
            {
                return new ValidationResult("El número de tarjeta no es válido.");
            }

            // Eliminar espacios o guiones si los hay
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            // Validar que solo tenga dígitos
            if (!Regex.IsMatch(cardNumber, @"^\d+$"))
            {
                return new ValidationResult("El número de tarjeta no es válido.");
            }

            // Validar longitud típica
            if (cardNumber.Length < 13 || cardNumber.Length > 19)
            {
                return new ValidationResult("El número de tarjeta no es válido.");
            }

            return ValidationResult.Success;
        }
    }
}