using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Application.Validations
{
    public class CancellationPolicyValidation
    {
        private ICancellationPolicyRepository _cancellationPolicyRepository;

        public CancellationPolicyValidation(ICancellationPolicyRepository cancellationPolicyRepository)
        {
            _cancellationPolicyRepository = cancellationPolicyRepository;
        }
        
        public async Task CancellationPolicyExistsAsync(int id)
        {
            try
            {
                await _cancellationPolicyRepository.GetByIdAsync(id);
            }
            catch
            {
                throw new ApiException(new ApiErrorResponse("The cancellation policy does not exist in the database."));
            }
        }
    }
}