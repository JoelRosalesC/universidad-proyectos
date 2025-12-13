using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.VehicleTypes
{
    public class GetVehicleTypeByIdUseCase
    {
        private IVehicleTypeRepository _vehicleTypeRepository;

        public GetVehicleTypeByIdUseCase(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<VehicleType> runAsync(int id)
        {
            return await _vehicleTypeRepository.GetByIdAsync(id);
        }
    }
}