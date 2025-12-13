
namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IImageStorageRepository
    {
        Task<string> SaveImageAsync(IFormFile image);
    }
}