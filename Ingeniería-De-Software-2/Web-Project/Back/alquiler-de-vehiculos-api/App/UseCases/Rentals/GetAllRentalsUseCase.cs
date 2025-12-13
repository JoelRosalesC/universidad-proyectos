using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.Employees
{
    public class GetAllRentalsUseCase
    {
        private readonly IRentalRepository _rentalRepository;


        public GetAllRentalsUseCase(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<List<Rental>> RunAsync()
        {
            return await _rentalRepository.GetAllAsync();
        }
    }
}