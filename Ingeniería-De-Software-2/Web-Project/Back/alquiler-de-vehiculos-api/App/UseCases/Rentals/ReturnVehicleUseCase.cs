using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class ReturnVehicleUseCase
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ReturnVehicleUseCase(IRentalRepository rentalRepository, IVehicleRepository vehicleRepository, IEmployeeRepository employeeRepository)
        {
            _rentalRepository = rentalRepository;
            _vehicleRepository = vehicleRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task RunAsync(ReturnVehicleDTO rentalDTO, int employeeId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalDTO.RentalId);
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            rental.Status = RentalStatus.Returned;
            rental.ReturnedEmployeeId = employeeId;
            rental.ReturnedBranchId = employee.WorkBranch;
            await _rentalRepository.UpdateAsync(rental);
            var vehicle = await _vehicleRepository.GetByIdAsync(rental.DeliveredVehicleId);
            vehicle.CurrentBranchId = employee.WorkBranch;
            await _vehicleRepository.UpdateAsync(vehicle);
        }
    }
}