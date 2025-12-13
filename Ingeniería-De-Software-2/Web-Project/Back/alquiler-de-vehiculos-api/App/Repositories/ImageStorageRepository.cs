
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class ImageStorageRepository : IImageStorageRepository
    {
        
        public async Task<string> SaveImageAsync(IFormFile image)
        {
            var uploadsFolder = Path.Combine("Assets", "Images", "Uploads");
            Directory.CreateDirectory(uploadsFolder); // Crea el directorio si no existe

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return fileName;
        }

    }
}