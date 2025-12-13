using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Entities.DTOs.VehicleTypes;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.VehicleTypes
{
    public class UpdateVehicleTypeUseCase
    {
        private IVehicleTypeRepository _vehicleTypeRepository;

        public UpdateVehicleTypeUseCase(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<VehicleType> runAsync(VehicleTypeUpdateDTO vehicleTypeDTO, string imageUrl)
        {
            var vehicleType = await _vehicleTypeRepository.GetByIdAsync(vehicleTypeDTO.Id);
            
            vehicleType.BrandId = vehicleTypeDTO.BrandId;
            vehicleType.Model = vehicleTypeDTO.Model;
            vehicleType.PassengerCapacity = int.Parse(vehicleTypeDTO.PassengerCapacity);
            vehicleType.PricePerDay = double.Parse(vehicleTypeDTO.PricePerDay);
            vehicleType.Category = vehicleTypeDTO.Category;
            vehicleType.CancellationPolicyId = vehicleTypeDTO.CancellationPolicyId;
            if(imageUrl != "")
                vehicleType.ImageUrl = imageUrl;

            vehicleType = await _vehicleTypeRepository.UpdateAsync(vehicleType);

            return vehicleType;
        }
    }
}