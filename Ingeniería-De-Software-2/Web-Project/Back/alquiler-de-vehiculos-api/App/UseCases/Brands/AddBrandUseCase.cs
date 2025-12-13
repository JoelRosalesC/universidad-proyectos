using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Brands;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Brands
{
    public class AddBrandUseCase
    {
        private IBrandRepository _brandRepository;

        public AddBrandUseCase(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Brand> runAsync(BrandAddDTO brandDTO)
        {
            var brand = new Brand
            {
                Name = brandDTO.Name
            };

            brand = await _brandRepository.AddAsync(brand);

            return brand;
        }
    }
}