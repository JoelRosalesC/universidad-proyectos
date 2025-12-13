using System.ComponentModel.DataAnnotations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals
{
    public class ReturnVehicleDTO
    {
        #nullable disable
        [Required]
        public int RentalId { get; set; }
        #nullable restore
    }
}