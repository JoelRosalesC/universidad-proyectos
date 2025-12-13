using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class VehicleType
    {
        #nullable disable
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Model { get; set; }
        public int PassengerCapacity { get; set; }
        public double PricePerDay { get; set; }
        public VehicleCategory Category { get; set; }
        public int CancellationPolicyId { get; set; }
        public string ImageUrl { get; set; }
        #nullable restore

    }
}