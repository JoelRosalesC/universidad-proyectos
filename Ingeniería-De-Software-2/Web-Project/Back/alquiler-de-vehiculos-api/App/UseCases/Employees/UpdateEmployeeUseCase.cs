using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class UpdateEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHashRepository _hashRepository;

        public UpdateEmployeeUseCase(IEmployeeRepository employeeRepository, IUserRepository userRepository, IHashRepository hashRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _hashRepository = hashRepository;
        }

        public async Task<EmployeeGetDTO> RunAsync(EmployeeUpdateDTO employeeDTO)
        {
            // Create user first
            var user = await _userRepository.GetByIdAsync(employeeDTO.Id);
            user.Mail = employeeDTO.Mail;
            
            
            user = await _userRepository.UpdateAsync(user);

            // Create employee with user ID
            var employee = await _employeeRepository.GetByIdAsync(user.Id);
            employee.FirstName = employeeDTO.FirstName;
            employee.LastName = employeeDTO.LastName;
            employee.Dni = employeeDTO.Dni;
            employee.PhoneNumber = employeeDTO.PhoneNumber;
            employee.Birthdate = employeeDTO.Birthdate;
            employee.WorkBranch = employeeDTO.WorkBranch;
            
            await _employeeRepository.UpdateAsync(employee);
            
            // Return DTO with combined information
            return new EmployeeGetDTO()
            {
                Id = user.Id,
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