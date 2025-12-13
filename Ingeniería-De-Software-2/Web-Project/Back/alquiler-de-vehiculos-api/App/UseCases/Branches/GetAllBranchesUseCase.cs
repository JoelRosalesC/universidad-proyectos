using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Branches
{
    public class GetAllBranchesUseCase
    {
        private IBranchRepository _branchRepository;

        public GetAllBranchesUseCase(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<List<Branch>> runAsync()
        {
            return await _branchRepository.GetAllAsync();
        }
    }
}