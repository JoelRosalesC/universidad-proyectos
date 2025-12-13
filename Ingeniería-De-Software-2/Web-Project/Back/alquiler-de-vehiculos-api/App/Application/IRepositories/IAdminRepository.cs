using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IAdminRepository
    {
        Task<Admin> AddAsync(Admin admin);
        Task<Admin> UpdateAsync(Admin admin);
        Task DeleteByIdAsync(int id);
        Task<List<Admin>> GetAllAsync();
        Task<Admin> GetByIdAsync(int id);
    }
}