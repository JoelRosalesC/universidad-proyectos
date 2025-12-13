using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Statistics;
using AlquilerDeVehiculosApi.App.Application.Entities.Models;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class GetDBUseCase
    {

        private IVehicleRepository _vehicleRepository;
        private IVehicleTypeRepository _vehicleTypeRepository;
        private IRentalRepository _rentalRepository;
        private IEmployeeRepository _employeeRepository;
        private IBranchRepository _branchRepository;
        private ICustomerRepository _customerRepository;
        private IUserRepository _userRepository;

        public GetDBUseCase(IVehicleRepository vehicleRepository, IVehicleTypeRepository vehicleTypeRepository, IRentalRepository rentalRepository, IEmployeeRepository employeeRepository, IBranchRepository branchRepository, ICustomerRepository customerRepository, IUserRepository userRepository)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
            _rentalRepository = rentalRepository;
            _employeeRepository = employeeRepository;
            _branchRepository = branchRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        public async Task<dynamic> runAsync()
        {
            var statistics = new
            {
                vehicles = await _vehicleRepository.GetAllAsync(),
                vehicleTypes = await _vehicleTypeRepository.GetAllAsync(),
                rentals = await _rentalRepository.GetAllAsync(),
                users = await _userRepository.GetAllAsync(),
                employees = await _employeeRepository.GetAllAsync(),
                customers = await _customerRepository.GetAllAsync(),
                branchs = await _branchRepository.GetAllAsync(),
            };

            return statistics;
        }
    }
}