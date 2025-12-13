using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IBranchRepository
    {
        Task<Branch> AddAsync(Branch branch);
        Task<Branch> UpdateAsync(Branch branch);
        Task DeleteByIdAsync(int id);
        Task<List<Branch>> GetAllAsync();
        Task<Branch> GetByIdAsync(int id);
    }
}