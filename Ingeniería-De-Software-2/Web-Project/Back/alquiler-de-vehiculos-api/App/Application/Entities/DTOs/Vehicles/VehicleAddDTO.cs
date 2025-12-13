using System.ComponentModel.DataAnnotations;
using AlquilerDeVehiculosApi.App.Application.Entities.Validations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Vehicles
{
    public class VehicleAddDTO
    {
        #nullable disable
        [Required]
        [StringLength(7, MinimumLength = 6, ErrorMessage = "Patente inválida")]
        public string LicensePlate { get; set; }
        
        [Required]
        public string Color { get; set; }

        [Required]
        [ValidYearAttribute]
        public string Year { get; set; }
        
        [Required]
        public int VehicleTypeId { get; set; }
        
        [Required]
        public int CurrentBranchId { get; set; }
        #nullable restore
    }
}