using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Vehicles;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.VehicleTypes;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.VehicleTypes
{
    public class UpdateVehicleUseCase
    {
        private IVehicleRepository _vehicleRepository;

        public UpdateVehicleUseCase(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Vehicle> runAsync(VehicleUpdateDTO vehicleDTO)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleDTO.Id);
            
            vehicle.LicensePlate = vehicleDTO.LicensePlate;
            vehicle.Color = vehicleDTO.Color;
            vehicle.Year = vehicleDTO.Year;
            vehicle.CurrentBranchId = vehicleDTO.CurrentBranchId;
            vehicle.VehicleTypeId = vehicleDTO.VehicleTypeId;

            vehicle = await _vehicleRepository.UpdateAsync(vehicle);

            return vehicle;
        }
    }
}