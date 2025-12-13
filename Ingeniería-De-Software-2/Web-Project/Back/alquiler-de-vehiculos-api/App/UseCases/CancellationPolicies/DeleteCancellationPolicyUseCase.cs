using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.CancellationPolicies
{
    public class DeleteCancellationPolicyUseCase
    {
        private ICancellationPolicyRepository _cancellationPolicyRepository;

        public DeleteCancellationPolicyUseCase(ICancellationPolicyRepository cancellationPolicyRepository)
        {
            _cancellationPolicyRepository = cancellationPolicyRepository;
        }

        public async Task runAsync(int id)
        {
            await _cancellationPolicyRepository.DeleteByIdAsync(id);
        }
    }
}