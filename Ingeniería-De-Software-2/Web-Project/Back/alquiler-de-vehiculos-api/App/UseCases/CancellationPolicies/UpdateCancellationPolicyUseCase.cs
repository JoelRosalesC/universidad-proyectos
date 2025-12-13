using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.CancellationPolicies;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.CancellationPolicies
{
    public class UpdateCancellationPolicyUseCase
    {
        private ICancellationPolicyRepository _cancellationPolicyRepository;

        public UpdateCancellationPolicyUseCase(ICancellationPolicyRepository cancellationPolicyRepository)
        {
            _cancellationPolicyRepository = cancellationPolicyRepository;
        }

        public async Task<CancellationPolicy> runAsync(CancellationPolicyUpdateDTO cancellationPolicyDTO)
        {
            var cancellationPolicy = await _cancellationPolicyRepository.GetByIdAsync(cancellationPolicyDTO.Id);
            
            cancellationPolicy.Description = cancellationPolicyDTO.Description;
            cancellationPolicy.ReturnPercentage = double.Parse(cancellationPolicyDTO.ReturnPercentage);

            cancellationPolicy = await _cancellationPolicyRepository.UpdateAsync(cancellationPolicy);

            return cancellationPolicy;
        }
    }
}