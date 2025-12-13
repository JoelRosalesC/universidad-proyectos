using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class DeleteVehicleUseCase
    {
        private IVehicleRepository _vehicleRepository;

        public DeleteVehicleUseCase(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task runAsync(int id)
        {
            await _vehicleRepository.DeleteByIdAsync(id);
        }
    }
}