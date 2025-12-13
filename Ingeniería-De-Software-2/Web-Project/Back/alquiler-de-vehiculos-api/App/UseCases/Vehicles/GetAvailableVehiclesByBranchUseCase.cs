
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class GetAvailableVehiclesByBranchUseCase
    {

        private IVehicleRepository _vehicleRepository;
        private IEmployeeRepository _employeeRepository;
        private IRentalRepository _rentalRepository;

        public GetAvailableVehiclesByBranchUseCase(IVehicleRepository vehicleRepository, IEmployeeRepository employeeRepository, IRentalRepository rentalRepository)
        {
            _vehicleRepository = vehicleRepository;
            _employeeRepository = employeeRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task<List<Vehicle>> runAsync(int employeeId)
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            var rentals = await _rentalRepository.GetAllAsync();

            vehicles = vehicles.Where(v => v.CurrentBranchId == employee.WorkBranch &&
                !rentals.Exists(r => r.DeliveredVehicleId == v.Id && r.Status == Application.Enums.RentalStatus.Rented)).ToList();

            return vehicles;
        }
    }
}