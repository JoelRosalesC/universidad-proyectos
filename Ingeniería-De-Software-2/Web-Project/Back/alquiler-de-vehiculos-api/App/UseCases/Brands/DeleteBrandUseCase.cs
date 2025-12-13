using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Brands
{
    public class DeleteBrandUseCase
    {
        private IBrandRepository _brandRepository;

        public DeleteBrandUseCase(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task runAsync(int id)
        {
            await _brandRepository.DeleteByIdAsync(id);
        }
    }
}