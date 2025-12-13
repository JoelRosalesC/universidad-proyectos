using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IAdditionalRepository
    {
        Task<Additional> AddAsync(Additional additional);
        Task<Additional> UpdateAsync(Additional additional);
        Task DeleteByIdAsync(int id);
        Task<List<Additional>> GetAllAsync();
        Task<Additional> GetByIdAsync(int id);
    }
}