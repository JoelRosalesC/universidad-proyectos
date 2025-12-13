using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class AddUserRentalUseCase
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AddUserRentalUseCase(IRentalRepository rentalRepository, IVehicleTypeRepository vehicleTypeRepository, IVehicleRepository vehicleRepository, IEmployeeRepository employeeRepository)
        {
            _rentalRepository = rentalRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
            _vehicleRepository = vehicleRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Rental> RunAsync(UserRentalAddDTO rentalDTO, int employeeId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(rentalDTO.DeliveredVehicleId);
            var vehicleType = await _vehicleTypeRepository.GetByIdAsync(vehicle.VehicleTypeId);
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            var newRental = new Rental()
            {
                RegistrationDate = DateTime.Now,
                RentalDate = DateTime.Now,
                ReturnDate = rentalDTO.EndDate.Date,
                SelectedVehicleTypeId = vehicle.VehicleTypeId,

                CancellationPolicyId = vehicleType.CancellationPolicyId,
                PickedUpBranchId = employee.WorkBranch,
                CustomerId = rentalDTO.CustomerId,
                TotalPrice = ((rentalDTO.EndDate - DateTime.Now).Days + 1) * vehicleType.PricePerDay,
                Status = RentalStatus.Rented,
                Additionals = string.Join(",", rentalDTO.Additionals.Select(s => ((int)s).ToString())),
                DeliveredVehicleId = rentalDTO.DeliveredVehicleId,
                PickedUpEmployeeId = employeeId      
            };
            await _rentalRepository.AddAsync(newRental);
            return newRental;
        }
    }
}