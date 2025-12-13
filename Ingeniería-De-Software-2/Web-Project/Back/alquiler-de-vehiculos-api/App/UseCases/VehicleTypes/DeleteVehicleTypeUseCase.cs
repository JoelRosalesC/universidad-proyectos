using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.VehicleTypes
{
    public class DeleteVehicleTypeUseCase
    {
        private IVehicleTypeRepository _vehicleTypeRepository;

        public DeleteVehicleTypeUseCase(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task runAsync(int id)
        {
            await _vehicleTypeRepository.DeleteByIdAsync(id);
        }
    }
}