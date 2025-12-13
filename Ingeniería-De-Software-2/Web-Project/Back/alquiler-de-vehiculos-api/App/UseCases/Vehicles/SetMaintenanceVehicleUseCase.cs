using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Vehicles;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.VehicleTypes;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.VehicleTypes
{
    public class SetMaintenanceVehicleUseCase
    {
        private IVehicleRepository _vehicleRepository;

        public SetMaintenanceVehicleUseCase(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Vehicle> runAsync(VehicleSetMaintenanceDTO vehicleDTO)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleDTO.Id);

            vehicle.IsUnderMaintenance = vehicleDTO.IsUnderMaintenance;

            vehicle = await _vehicleRepository.UpdateAsync(vehicle);

            return vehicle;
        }
    }
}