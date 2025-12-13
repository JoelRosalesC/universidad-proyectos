using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Statistics;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class GetRentedVehiclesUseCase
    {

        private IVehicleRepository _vehicleRepository;
        private IRentalRepository _rentalRepository;

        public GetRentedVehiclesUseCase(IVehicleRepository vehicleRepository, IRentalRepository rentalRepository)
        {
            _vehicleRepository = vehicleRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task<List<Vehicle>> runAsync(StatisticsDateRangeDTO statisticsDTO)
        {
            var rentals = await _rentalRepository.GetAllAsync();
            var vehiclesId = rentals.Where(r => r.RentalDate <= statisticsDTO.EndDate && r.ReturnDate >= statisticsDTO.StartDate).Select(r => r.DeliveredVehicleId).Distinct().ToList();
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.Where(v => vehiclesId.Contains(v.Id)).ToList();
        }
    }
}