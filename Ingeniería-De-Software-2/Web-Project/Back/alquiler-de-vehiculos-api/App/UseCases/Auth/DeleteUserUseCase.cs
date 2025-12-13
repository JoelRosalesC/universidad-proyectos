

using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class DeleteUserUseCase
    {
        private IUserRepository _userRepository;

        public DeleteUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task runAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            user.Status = UserStatus.Deleted;
            await _userRepository.UpdateAsync(user);
        }
    }
}