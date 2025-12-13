using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IBrandRepository
    {
        Task<Brand> AddAsync(Brand brand);
        Task<Brand> UpdateAsync(Brand brand);
        Task DeleteByIdAsync(int id);
        Task<List<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(int id);
    }
}