using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.VehicleTypes
{
    public class GetVehicleByIdUseCase
    {
        private IVehicleRepository _vehicleRepository;

        public GetVehicleByIdUseCase(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Vehicle> runAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }
    }
}