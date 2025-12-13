using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class InvalidateRentalUseCase
    {
        private readonly IRentalRepository _rentalRepository;

        public InvalidateRentalUseCase(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task RunAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            rental.Status = RentalStatus.Invalidated;
            await _rentalRepository.UpdateAsync(rental);
        }
    }
}