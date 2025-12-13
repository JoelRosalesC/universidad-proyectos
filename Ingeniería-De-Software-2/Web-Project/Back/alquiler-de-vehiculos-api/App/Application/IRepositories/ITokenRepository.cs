using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.Models;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface ITokenRepository
    {
        string Generate(User user);
        string GenerateRefreshToken();
    }
}