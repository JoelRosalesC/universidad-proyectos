using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Brands
{
    public class GetAllBrandsUseCase
    {
        private IBrandRepository _brandRepository;

        public GetAllBrandsUseCase(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<List<Brand>> runAsync()
        {
            return await _brandRepository.GetAllAsync();
        }
    }
}