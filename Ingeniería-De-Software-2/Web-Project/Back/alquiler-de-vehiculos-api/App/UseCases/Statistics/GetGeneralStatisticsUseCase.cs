using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Statistics;
using AlquilerDeVehiculosApi.App.Application.Entities.Models;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class GetGeneralStatisticsUseCase
    {

        private IVehicleRepository _vehicleRepository;
        private IVehicleTypeRepository _vehicleTypeRepository;
        private IRentalRepository _rentalRepository;
        private IEmployeeRepository _employeeRepository;
        private IBranchRepository _branchRepository;
        private ICustomerRepository _customerRepository;

        public GetGeneralStatisticsUseCase(IVehicleRepository vehicleRepository, IVehicleTypeRepository vehicleTypeRepository, IRentalRepository rentalRepository, IEmployeeRepository employeeRepository, IBranchRepository branchRepository, ICustomerRepository customerRepository)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
            _rentalRepository = rentalRepository;
            _employeeRepository = employeeRepository;
            _branchRepository = branchRepository;
            _customerRepository = customerRepository;
        }

        public async Task<GeneralStatistics> runAsync(StatisticsDateRangeDTO statisticsDTO)
        {
            var statistics = new GeneralStatistics();
            var vehicles = await _vehicleRepository.GetAllAsync();
            var vehicleTypes = await _vehicleTypeRepository.GetAllAsync();
            var rentals = await _rentalRepository.GetAllAsync();
            var employees = await _employeeRepository.GetAllAsync();
            var branchs = await _branchRepository.GetAllAsync();
            var customers = await _customerRepository.GetAllAsync();
            statistics.TotalVehicles = vehicles.Count();
            statistics.EmployeesWhoSoldTheMost = employees.OrderBy(e => rentals.Where(r => r.PickedUpEmployeeId == e.UserId).Count()).ToList();
            statistics.MostChosenBranch = branchs.OrderBy(b => rentals.Where(r => r.PickedUpBranchId == b.Id).Count()).Select(b => b.Name).ToList();
            statistics.MostRentedVehicleType = vehicleTypes.OrderBy(v => rentals.Where(r => r.SelectedVehicleTypeId == v.Id).Count()).ToList();

            var startDate = statisticsDTO.StartDate.Date; // 00:00:00 del día
            var endDate = statisticsDTO.EndDate.Date.AddDays(1).AddTicks(-1); // 23:59:59.9999999 del día

            statistics.RegisteredCustomers = customers.Where(c => c.CreationDate >= startDate && c.CreationDate <= endDate).Count();
            return statistics;
        }
    }
}