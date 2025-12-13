
using Microsoft.EntityFrameworkCore;

namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    public class Vehicle
    {
        #nullable disable
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }
        public int VehicleTypeId { get; set; }
        public int CurrentBranchId { get; set; }
        public bool IsUnderMaintenance { get; set; }

        #nullable restore

    }
}