using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.CancellationPolicies
{
    public class GetAllCancellationPoliciesUseCase
    {
        private ICancellationPolicyRepository _cancellationPolicyRepository;

        public GetAllCancellationPoliciesUseCase(ICancellationPolicyRepository cancellationPolicyRepository)
        {
            _cancellationPolicyRepository = cancellationPolicyRepository;
        }

        public async Task<List<CancellationPolicy>> runAsync()
        {
            return await _cancellationPolicyRepository.GetAllAsync();
        }
    }
}