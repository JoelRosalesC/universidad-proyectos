using System.ComponentModel.DataAnnotations;
using AlquilerDeVehiculosApi.App.Application.Entities.Validations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Vehicles
{
    public class VehicleSetMaintenanceDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool IsUnderMaintenance { get; set; }
    }
}