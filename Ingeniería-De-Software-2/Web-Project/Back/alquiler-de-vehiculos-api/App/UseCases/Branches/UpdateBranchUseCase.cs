using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Branches;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Branches
{
    public class UpdateBranchUseCase
    {
        private IBranchRepository _branchRepository;

        public UpdateBranchUseCase(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Branch> runAsync(BranchUpdateDTO branchDTO)
        {
            var branch = await _branchRepository.GetByIdAsync(branchDTO.Id);
            
            branch.Name = branchDTO.Name;
            branch.Province = branchDTO.Province;
            branch.Locality = branchDTO.Locality;

            branch = await _branchRepository.UpdateAsync(branch);

            return branch;
        }
    }
}