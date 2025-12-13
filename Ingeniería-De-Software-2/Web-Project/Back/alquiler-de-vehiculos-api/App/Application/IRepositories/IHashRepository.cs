using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IHashRepository
    {
        public (string hash, string salt) HashPassword(string password);
        public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt);
    }
}