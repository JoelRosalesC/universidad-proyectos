using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Employees;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Rentals;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class GetAllRentalsByCustomerUseCase
    {
        private readonly IRentalRepository _rentalRepository;

        public GetAllRentalsByCustomerUseCase(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<List<Rental>> RunAsync(int customerId)
        {
            return await _rentalRepository.GetAllByCustomerIdAsync(customerId);
        }
    }
}