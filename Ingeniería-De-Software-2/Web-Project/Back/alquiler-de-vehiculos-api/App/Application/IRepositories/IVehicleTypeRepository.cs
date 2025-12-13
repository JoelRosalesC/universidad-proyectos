using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IVehicleTypeRepository
    {
        Task<VehicleType> AddAsync(VehicleType vehicleType);
        Task<VehicleType> UpdateAsync(VehicleType vehicleType);
        Task DeleteByIdAsync(int id);
        Task<List<VehicleType>> GetAllAsync();
        Task<VehicleType> GetByIdAsync(int id);
    }
}