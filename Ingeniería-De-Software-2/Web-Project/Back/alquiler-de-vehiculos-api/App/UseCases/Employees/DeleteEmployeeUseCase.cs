using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class DeleteEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;

        public DeleteEmployeeUseCase(IEmployeeRepository employeeRepository, IUserRepository userRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        public async Task RunAsync(int id)
        {
            // Get the user to update status
            var user = await _userRepository.GetByIdAsync(id);
            user.Status = UserStatus.Deleted;
            await _userRepository.UpdateAsync(user);
            
            // Delete the employee record
            // await _employeeRepository.DeleteByIdAsync(id);
        }
    }
}