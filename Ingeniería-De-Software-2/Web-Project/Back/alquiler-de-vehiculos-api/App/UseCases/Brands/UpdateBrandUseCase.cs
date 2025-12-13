using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Brands;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Brands
{
    public class UpdateBrandUseCase
    {
        private IBrandRepository _brandRepository;

        public UpdateBrandUseCase(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Brand> runAsync(BrandUpdateDTO brandDTO)
        {
            var brand = await _brandRepository.GetByIdAsync(brandDTO.Id);
            brand.Name = brandDTO.Name;

            brand = await _brandRepository.UpdateAsync(brand);

            return brand;
        }
    }
}