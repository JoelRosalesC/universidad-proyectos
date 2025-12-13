using System.ComponentModel.DataAnnotations;
using AlquilerDeVehiculosApi.App.Application.Entities.Validations;
using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals
{
    public class UserRentalAddDTO
    {
#nullable disable
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int DeliveredVehicleId { get; set; }
        public List<AdditionalEnum> Additionals { get; set; }
#nullable restore
    }
}