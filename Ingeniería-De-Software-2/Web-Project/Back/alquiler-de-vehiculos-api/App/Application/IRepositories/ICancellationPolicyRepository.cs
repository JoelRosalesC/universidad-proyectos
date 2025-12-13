using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface ICancellationPolicyRepository
    {
        Task<CancellationPolicy> AddAsync(CancellationPolicy cancellationPolicy);
        Task<CancellationPolicy> UpdateAsync(CancellationPolicy cancellationPolicy);
        Task DeleteByIdAsync(int id);
        Task<List<CancellationPolicy>> GetAllAsync();
        Task<CancellationPolicy> GetByIdAsync(int id);
    }
}