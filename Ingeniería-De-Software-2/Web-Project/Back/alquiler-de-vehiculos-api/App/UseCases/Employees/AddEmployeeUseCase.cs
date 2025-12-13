using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class AddEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHashRepository _hashRepository;

        public AddEmployeeUseCase(IEmployeeRepository employeeRepository, IUserRepository userRepository, IHashRepository hashRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _hashRepository = hashRepository;
        }

        public async Task<EmployeeGetDTO> RunAsync(EmployeeAddDTO employeeDTO)
        {
            // Create user first
            User newUser = new User();
            newUser.Status = UserStatus.Active;
            newUser.Mail = employeeDTO.Mail;
            newUser.Role = UserRole.Employee;
            
            var (hash, salt) = _hashRepository.HashPassword(employeeDTO.Password);
            newUser.Password = hash;
            newUser.Salt = salt;
            
            newUser = await _userRepository.AddAsync(newUser);
            
            // Create employee with user ID
            Employee employee = new Employee()
            {
                UserId = newUser.Id,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                Dni = employeeDTO.Dni,
                PhoneNumber = employeeDTO.PhoneNumber,
                Birthdate = employeeDTO.Birthdate,
                CreationDate = DateTime.Now,
                WorkBranch = employeeDTO.WorkBranch
            };
            
            await _employeeRepository.AddAsync(employee);
            
            // Return DTO with combined information
            return new EmployeeGetDTO()
            {
                Id = newUser.Id,
                Mail = newUser.Mail,
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