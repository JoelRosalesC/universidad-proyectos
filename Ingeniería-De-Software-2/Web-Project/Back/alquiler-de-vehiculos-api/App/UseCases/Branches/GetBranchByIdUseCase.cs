using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Brands
{
    public class GetBranchByIdUseCase
    {
        private IBranchRepository _branchRepository;

        public GetBranchByIdUseCase(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Branch> runAsync(int id)
        {
            return await _branchRepository.GetByIdAsync(id);
        }
    }
}