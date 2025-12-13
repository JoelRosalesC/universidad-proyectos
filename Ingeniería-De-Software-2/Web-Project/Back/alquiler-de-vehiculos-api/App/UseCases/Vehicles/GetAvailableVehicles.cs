
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class GetAvailableVehiclesUseCase
    {

        private IVehicleRepository _vehicleRepository;

        public GetAvailableVehiclesUseCase(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<List<Vehicle>> runAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }
    }
}