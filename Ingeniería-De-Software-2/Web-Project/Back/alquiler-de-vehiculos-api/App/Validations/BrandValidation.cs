using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Application.Validations
{
    public class BrandValidation
    {
        private IBrandRepository _brandRepository;

        public BrandValidation(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        
        public async Task BrandExistsAsync(int id)
        {
            try
            {
                await _brandRepository.GetByIdAsync(id);
            }
            catch
            {
                throw new ApiException(new ApiErrorResponse("The brand does not exist in the database."));
            }
        }
        
        public async Task BrandNameAlreadyExistsAsync(string name)
        {
            var brands = await _brandRepository.GetAllAsync();
            var existingBrand = brands.FirstOrDefault(b => b.Name.ToLower() == name.ToLower());
            
            if (existingBrand != null)
            {
                throw new ApiException(new ApiErrorResponse("A brand with this name already exists."));
            }
        }
    }
}