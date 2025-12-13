using System.ComponentModel.DataAnnotations;
using AlquilerDeVehiculosApi.App.Application.Entities.Validations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals
{
    public class RentalAddDTO
    {
#nullable disable
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int VehicleTypeId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [CardNumber(ErrorMessage = "El número de tarjeta no es válido.")]
        public string CardNumber { get; set; }
        [Required]
        [ExpirationDateAttribute(ErrorMessage = "La fecha debe estar en formato MM/yy y ser futura.")]
        public string ExpirationDate { get; set; }
        [Required]
        [CvvCode(ErrorMessage = "El CVV no es valido.")]
        public string CvvCode { get; set; }
#nullable restore
    }
}