using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class GetEmployeeByIdUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;

        public GetEmployeeByIdUseCase(IEmployeeRepository employeeRepository, IUserRepository userRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        public async Task<EmployeeGetDTO> RunAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            var user = await _userRepository.GetByIdAsync(id);
            
            return new EmployeeGetDTO
            {
                Id = employee.UserId,
                Mail = user.Mail,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Dni = employee.Dni,
                Birthdate = employee.Birthdate,
                PhoneNumber = employee.PhoneNumber,
                CreationDate = employee.CreationDate,
                WorkBranch = employee.WorkBranch
            };
        }
    }
}