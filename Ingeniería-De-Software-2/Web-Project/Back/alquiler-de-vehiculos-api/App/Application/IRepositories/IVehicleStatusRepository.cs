using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IVehicleStatusRepository
    {
        Task<VehicleStatus> AddAsync(VehicleStatus vehicleStatus);
        Task<VehicleStatus> UpdateAsync(VehicleStatus vehicleStatus);
        Task DeleteByIdAsync(int id);
        Task<List<VehicleStatus>> GetAllAsync();
        Task<VehicleStatus> GetByIdAsync(int id);
    }
}