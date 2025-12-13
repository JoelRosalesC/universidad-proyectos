using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.CancellationPolicies
{
    public class GetCancellationPolicyByIdUseCase
    {
        private ICancellationPolicyRepository _cancellationPolicyRepository;

        public GetCancellationPolicyByIdUseCase(ICancellationPolicyRepository cancellationPolicyRepository)
        {
            _cancellationPolicyRepository = cancellationPolicyRepository;
        }

        public async Task<CancellationPolicy> runAsync(int id)
        {
            return await _cancellationPolicyRepository.GetByIdAsync(id);
        }
    }
}