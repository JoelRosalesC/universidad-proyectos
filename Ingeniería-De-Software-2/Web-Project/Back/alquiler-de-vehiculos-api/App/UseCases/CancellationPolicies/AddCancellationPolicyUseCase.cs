using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.CancellationPolicies;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.CancellationPolicies
{
    public class AddCancellationPolicyUseCase
    {
        private ICancellationPolicyRepository _cancellationPolicyRepository;

        public AddCancellationPolicyUseCase(ICancellationPolicyRepository cancellationPolicyRepository)
        {
            _cancellationPolicyRepository = cancellationPolicyRepository;
        }

        public async Task<CancellationPolicy> runAsync(CancellationPolicyAddDTO cancellationPolicyDTO)
        {
            var cancellationPolicy = new CancellationPolicy
            {
                Description = cancellationPolicyDTO.Description,
                ReturnPercentage = double.Parse(cancellationPolicyDTO.ReturnPercentage)
            };

            cancellationPolicy = await _cancellationPolicyRepository.AddAsync(cancellationPolicy);

            return cancellationPolicy;
        }
    }
}