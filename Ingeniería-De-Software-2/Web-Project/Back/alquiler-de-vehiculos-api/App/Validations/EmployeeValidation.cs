using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.Application.Validations
{
    public class EmployeeValidation
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;

        public EmployeeValidation(IEmployeeRepository employeeRepository, IUserRepository userRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        public async Task EmailAlreadyExistsAsync(string mail)
        {
            try
            {
                await _userRepository.GetByMailAsync(mail);
                throw new ApiException(new ApiErrorResponse("El correo electrónico ya está registrado. Por favor, usa otro."));
            }
            catch (ApiException ex)
            {
                // If the exception is from our check, rethrow it
                if (ex.Error != null && ex.Error.Errors.ContainsKey("generalError") &&
                    ex.Error.Errors["generalError"][0] == "El correo electrónico ya está registrado. Por favor, usa otro.")
                {
                    throw;
                }
                // Otherwise, the email doesn't exist, which is what we want
                return;
            }
        }

        public async Task DniAlreadyExistsAsync(string dni)
        {
            var employees = await _employeeRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();
            var employeesForDni = employees.Where(e => e.Dni == dni);
            var exists = users.Any(u => u.Status == UserStatus.Active && employeesForDni.Any(e => e.UserId == u.Id));

            if (exists)
            {
                throw new ApiException(new ApiErrorResponse("El DNI ya está registrado en el sistema."));
            }
        }

        public async Task EmployeeExistsAsync(int id)
        {
            try
            {
                await _employeeRepository.GetByIdAsync(id);
            }
            catch (ApiException)
            {
                throw new ApiException(new ApiErrorResponse("El empleado no existe en el sistema."));
            }
        }

        public async Task EmailAlreadyExistsUpdateAsync(int employeeId, string employeeMail)
        {
            try
            {
                var employee = await _userRepository.GetByMailAsync(employeeMail);
                if (employee.Id == employeeId)
                    return;
                throw new ApiException(new ApiErrorResponse("El correo electrónico ya está registrado. Por favor, usa otro."));
            }
            catch (ApiException ex)
            {
                // If the exception is from our check, rethrow it
                if (ex.Error != null && ex.Error.Errors.ContainsKey("generalError") &&
                    ex.Error.Errors["generalError"][0] == "El correo electrónico ya está registrado. Por favor, usa otro.")
                {
                    throw;
                }
                // Otherwise, the email doesn't exist, which is what we want
                return;
            }
        }
        public async Task DniAlreadyExistsUpdateAsync(int employeeId,string dni)
        {
            var employees = await _employeeRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();
            var employeesForDni = employees.Where(e => e.Dni == dni && e.UserId != employeeId);
            var exists = users.Any(u => u.Status == UserStatus.Active && employeesForDni.Any(e => e.UserId == u.Id));

            if (exists)
            {
                throw new ApiException(new ApiErrorResponse("El DNI ya está registrado en el sistema."));
            }
        }
        
    }
}