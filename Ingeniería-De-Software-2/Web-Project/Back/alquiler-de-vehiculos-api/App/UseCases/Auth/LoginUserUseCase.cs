
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class LoginUserUseCase
    {
        private ITokenRepository _tokenRepository;

        public LoginUserUseCase(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public string Run(User user)
        {
            string token = _tokenRepository.Generate(user);
            return token;
        }
    }
}