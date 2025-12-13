using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.VehicleTypes
{
    public class GetAllVehicleTypesUseCase
    {
        private IVehicleTypeRepository _vehicleTypeRepository;

        public GetAllVehicleTypesUseCase(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<List<VehicleType>> runAsync()
        {
            return await _vehicleTypeRepository.GetAllAsync();
        }
    }
}