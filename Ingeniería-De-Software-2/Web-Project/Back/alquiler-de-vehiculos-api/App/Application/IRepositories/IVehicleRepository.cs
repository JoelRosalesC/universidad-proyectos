
using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> AddAsync(Vehicle vehicle);
        Task<Vehicle> UpdateAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByLicensePlateAsync(string licensePlate);
        Task<Vehicle> GetByIdAsync(int id);
        Task DeleteByIdAsync(int id);
    }
}