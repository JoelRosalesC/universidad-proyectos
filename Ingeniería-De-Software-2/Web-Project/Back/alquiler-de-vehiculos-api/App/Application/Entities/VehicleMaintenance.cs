using AlquilerDeVehiculosApi.App.Application.Enums;
using Microsoft.EntityFrameworkCore;

namespace AlquilerDeVehiculosApi.App.Application.Entities
{
    [PrimaryKey(nameof(VehicleId), nameof(MaintenanceId))]
    public class VehicleMaintenance
    {
        #nullable disable
        public int VehicleId { get; set; }
        public int MaintenanceId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public MaintenanceStatus Status { get; set; }

        #nullable restore

    }
}