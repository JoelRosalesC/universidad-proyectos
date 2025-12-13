using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IAdditionalDriverRepository
    {
        Task<AdditionalDriver> AddAsync(AdditionalDriver additionalDriver);
        Task<AdditionalDriver> UpdateAsync(AdditionalDriver additionalDriver);
        Task DeleteByIdAsync(int id);
        Task<List<AdditionalDriver>> GetAllAsync();
        Task<AdditionalDriver> GetByIdAsync(int id);
    }
}