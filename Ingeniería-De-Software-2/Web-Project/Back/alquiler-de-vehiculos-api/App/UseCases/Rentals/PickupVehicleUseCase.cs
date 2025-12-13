using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class PickupVehicleUseCase
    {
        private readonly IRentalRepository _rentalRepository;

        public PickupVehicleUseCase(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task RunAsync(PickupVehicleDTO rentalDTO, int employeeId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalDTO.RentalId);
            rental.Status = RentalStatus.Rented;
            rental.PickedUpEmployeeId = employeeId;
            rental.DeliveredVehicleId = rentalDTO.DeliveredVehicleId;
            rental.Additionals = string.Join(",", rentalDTO.Additionals.Select(s => ((int)s).ToString()));
            await _rentalRepository.UpdateAsync(rental);
        }
    }
}