
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class SaveImageUseCase
    {

        private IImageStorageRepository _imageStorageRepository;
        public SaveImageUseCase(IImageStorageRepository imageStorageRepository)
        {
            _imageStorageRepository = imageStorageRepository;
        }

        public async Task<string> runAsync(IFormFile image)
        {
            return await _imageStorageRepository.SaveImageAsync(image);
        }
    }
}