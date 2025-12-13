using System.ComponentModel.DataAnnotations;
using AlquilerDeVehiculosApi.App.Application.Enums;
using Microsoft.AspNetCore.Mvc;
using AlquilerDeVehiculosApi.App.Application.Entities.Validations;

namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.VehicleTypes
{
    public class VehicleTypeAddDTO
    {
        [Required]
        public int BrandId { get; set; }
        
        [Required]
        public string Model { get; set; }
        
        [Required]
        [ValidPassengerCapacityAttribute]
        public string PassengerCapacity { get; set; }
        
        [Required]
        [ValidPricePerDayAttribute]
        public string PricePerDay { get; set; }
        
        [Required]
        public VehicleCategory Category { get; set; }
        
        [Required]
        public int CancellationPolicyId { get; set; }
        [FromForm(Name = "image")]
        public IFormFile Image { get; set; }
    }
}