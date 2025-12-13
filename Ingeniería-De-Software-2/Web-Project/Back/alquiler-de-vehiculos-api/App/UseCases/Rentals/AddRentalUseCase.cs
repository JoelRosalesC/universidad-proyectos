using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class AddRentalUseCase
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public AddRentalUseCase(IRentalRepository rentalRepository, IVehicleTypeRepository vehicleTypeRepository)
        {
            _rentalRepository = rentalRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<Rental> RunAsync(RentalAddDTO rentalDTO, int customerId)
        {
            var vehicleType = await _vehicleTypeRepository.GetByIdAsync(rentalDTO.VehicleTypeId);
            var newRental = new Rental()
            {
                RegistrationDate = DateTime.Now,
                RentalDate = rentalDTO.StartDate.Date,
                ReturnDate = rentalDTO.EndDate.Date,
                SelectedVehicleTypeId = rentalDTO.VehicleTypeId,
                CancellationPolicyId = vehicleType.CancellationPolicyId,
                PickedUpBranchId = rentalDTO.BranchId,
                CustomerId = customerId,
                TotalPrice = ((rentalDTO.EndDate - rentalDTO.StartDate).Days + 1) * vehicleType.PricePerDay,
                Status = RentalStatus.Pending
            };
            await _rentalRepository.AddAsync(newRental);
            return newRental;
        }
    }
}