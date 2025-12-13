namespace AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Vehicles
{
    public class VehicleDTO
    {
        #nullable disable
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }
        public int VehicleTypeId { get; set; }
        public int CurrentBranchId { get; set; }
        #nullable restore
    }
}