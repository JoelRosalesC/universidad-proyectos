using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.VehicleTypes;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.VehicleTypes
{
    public class AddVehicleTypeUseCase
    {
        private IVehicleTypeRepository _vehicleTypeRepository;

        public AddVehicleTypeUseCase(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<VehicleType> runAsync(VehicleTypeAddDTO vehicleTypeDTO, string imageUrl)
        {
            var vehicleType = new VehicleType
            {
                BrandId = vehicleTypeDTO.BrandId,
                Model = vehicleTypeDTO.Model,
                PassengerCapacity = int.Parse(vehicleTypeDTO.PassengerCapacity),
                PricePerDay = Double.Parse(vehicleTypeDTO.PricePerDay),
                Category = vehicleTypeDTO.Category,
                CancellationPolicyId = vehicleTypeDTO.CancellationPolicyId,
                ImageUrl = imageUrl
            };

            vehicleType = await _vehicleTypeRepository.AddAsync(vehicleType);

            return vehicleType;
        }
    }
}