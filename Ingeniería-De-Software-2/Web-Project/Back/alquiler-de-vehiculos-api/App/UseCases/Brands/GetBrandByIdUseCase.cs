using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Brands
{
    public class GetBrandByIdUseCase
    {
        private IBrandRepository _brandRepository;

        public GetBrandByIdUseCase(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Brand> runAsync(int id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }
    }
}