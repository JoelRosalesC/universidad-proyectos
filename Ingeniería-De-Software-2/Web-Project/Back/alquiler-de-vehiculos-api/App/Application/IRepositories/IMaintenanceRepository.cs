using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IMaintenanceRepository
    {
        Task<Maintenance> AddAsync(Maintenance maintenance);
        Task<Maintenance> UpdateAsync(Maintenance maintenance);
        Task DeleteByIdAsync(int id);
        Task<List<Maintenance>> GetAllAsync();
        Task<Maintenance> GetByIdAsync(int id);
    }
}