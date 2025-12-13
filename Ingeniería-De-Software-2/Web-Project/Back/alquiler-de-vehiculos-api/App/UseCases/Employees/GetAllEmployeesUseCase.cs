using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class GetAllEmployeesUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;

        public GetAllEmployeesUseCase(IEmployeeRepository employeeRepository, IUserRepository userRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        public async Task<List<EmployeeGetDTO>> RunAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var employeeDTOs = new List<EmployeeGetDTO>();

            foreach (var employee in employees)
            {
                try
                {
                    var user = await _userRepository.GetByIdAsync(employee.UserId);

                    employeeDTOs.Add(new EmployeeGetDTO
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
                    });
                }
                catch (ApiException)
                {

                }
            }

            return employeeDTOs;
        }
    }
}