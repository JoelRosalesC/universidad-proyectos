using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class GetRentalsInBranchUseCase
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IEmployeeRepository _employeeRepository;


        public GetRentalsInBranchUseCase(IRentalRepository rentalRepository, IEmployeeRepository employeeRepository)
        {
            _rentalRepository = rentalRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<List<Rental>> RunAsync(int userId)
        {
            var employee = await _employeeRepository.GetByIdAsync(userId);
            return await _rentalRepository.GetRentalsInBranch(employee.WorkBranch);
        }
    }
}