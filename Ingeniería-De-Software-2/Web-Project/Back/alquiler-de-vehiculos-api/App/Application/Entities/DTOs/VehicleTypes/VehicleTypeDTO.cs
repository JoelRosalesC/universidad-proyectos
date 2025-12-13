using AlquilerDeVehiculosApi.App.Application.Enums;
using Microsoft.AspNetCore.Mvc;


namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.VehicleTypes
{
    public class VehicleTypeDTO
    {
        #nullable disable
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Model { get; set; }
        public int PassengerCapacity { get; set; }
        public double PricePerDay { get; set; }
        public VehicleCategory Category { get; set; }
        public int CancellationPolicyId { get; set; }
        [FromForm(Name = "image")]
        public IFormFile Image { get; set; }
        #nullable restore
    }
}