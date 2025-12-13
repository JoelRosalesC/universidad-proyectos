using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Branches
{
    public class DeleteBranchUseCase
    {
        private IBranchRepository _branchRepository;

        public DeleteBranchUseCase(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task runAsync(int id)
        {
            await _branchRepository.DeleteByIdAsync(id);
        }
    }
}