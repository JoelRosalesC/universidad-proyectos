using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Branches;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Branches
{
    public class AddBranchUseCase
    {
        private IBranchRepository _branchRepository;

        public AddBranchUseCase(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Branch> runAsync(BranchAddDTO branchDTO)
        {
            var branch = new Branch
            {
                Name = branchDTO.Name,
                Province = branchDTO.Province,
                Locality = branchDTO.Locality
            };

            branch = await _branchRepository.AddAsync(branch);

            return branch;
        }
    }
}