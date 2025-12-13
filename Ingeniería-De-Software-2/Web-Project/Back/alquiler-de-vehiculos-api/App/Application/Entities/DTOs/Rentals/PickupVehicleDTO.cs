using System.ComponentModel.DataAnnotations;
using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals
{
    public class PickupVehicleDTO
    {
        #nullable disable
        [Required]
        public int RentalId { get; set; }
        public int DeliveredVehicleId { get; set; }
        public List<AdditionalEnum> Additionals { get; set; }
        #nullable restore
    }
}