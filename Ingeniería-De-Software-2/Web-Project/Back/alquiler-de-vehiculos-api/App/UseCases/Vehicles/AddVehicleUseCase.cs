
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.Vehicles;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class AddVehicleUseCase
    {

        private IVehicleRepository _vehicleRepository;

        public AddVehicleUseCase(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Vehicle> runAsync(VehicleAddDTO vehicleDTO)
        {
            var vehicle = new Vehicle()
            {
                LicensePlate = vehicleDTO.LicensePlate,
                Color = vehicleDTO.Color,
                Year = vehicleDTO.Year,
                VehicleTypeId = vehicleDTO.VehicleTypeId,
                CurrentBranchId = vehicleDTO.CurrentBranchId
            };

            vehicle = await _vehicleRepository.AddAsync(vehicle);

            return vehicle;
        }
    }
}