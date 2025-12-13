
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Auth;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class ResetPasswordUserUseCase
    {
        private IUserRepository _userRepository;
        private IHashRepository _hashRepository;
        public ResetPasswordUserUseCase(IUserRepository userRepository, IHashRepository hashRepository)
        {
            _userRepository = userRepository;
            _hashRepository = hashRepository;
        }

        public async Task runAsync(UserResetPasswordDTO userDTO)
        {
            User user = await _userRepository.GetByMailAsync(userDTO.Mail);
            var (hash, salt) = _hashRepository.HashPassword(userDTO.Password);
            user.Password = hash;
            user.Salt = salt;
            await _userRepository.UpdateAsync(user);
            
        }
    }
}