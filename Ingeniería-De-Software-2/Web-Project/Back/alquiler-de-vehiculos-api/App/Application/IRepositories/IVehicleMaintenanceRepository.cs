using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IVehicleMaintenanceRepository
    {
        Task<VehicleMaintenance> AddAsync(VehicleMaintenance vehicleMaintenance);
        Task<VehicleMaintenance> UpdateAsync(VehicleMaintenance vehicleMaintenance);
        Task DeleteByIdAsync(int vehicleId, int maintenanceId);
        Task<List<VehicleMaintenance>> GetAllAsync();
        Task<VehicleMaintenance> GetByIdAsync(int vehicleId, int maintenanceId);
    }
}