using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Application.Validations
{
    public class VehicleTypeValidation
    {
        private IVehicleTypeRepository _vehicleTypeRepository;
        private IBrandRepository _brandRepository;
        private ICancellationPolicyRepository _cancellationPolicyRepository;

        private string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

        public VehicleTypeValidation(
            IVehicleTypeRepository vehicleTypeRepository,
            IBrandRepository brandRepository,
            ICancellationPolicyRepository cancellationPolicyRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
            _brandRepository = brandRepository;
            _cancellationPolicyRepository = cancellationPolicyRepository;
        }

        public void ImageRequired(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new ApiException(new ApiErrorResponse("imagen es requerida"));
        }
        public void FileFormatNotSupported(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return;
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new ApiException(new ApiErrorResponse("formato de la imagen no soportado"));
                
        }
        
        public async Task VehicleTypeExistsAsync(int id)
        {
            try
            {
                await _vehicleTypeRepository.GetByIdAsync(id);
            }
            catch
            {
                throw new ApiException(new ApiErrorResponse("The vehicle type does not exist in the database."));
            }
        }
        public async Task VehicleTypeRepeatedAsync(int brandId, string model, int passengerCapacity)
        {
            var vehicleTypes = await _vehicleTypeRepository.GetAllAsync();
            if(vehicleTypes.Exists(v => v.BrandId == brandId && v.Model == model && v.PassengerCapacity == passengerCapacity))
                throw new ApiException(new ApiErrorResponse("ya existe ese tipo de vehiculo"));
        }
        
        public async Task BrandExistsAsync(int brandId)
        {
            try
            {
                await _brandRepository.GetByIdAsync(brandId);
            }
            catch
            {
                throw new ApiException(new ApiErrorResponse("The brand does not exist in the database."));
            }
        }
        
        public async Task CancellationPolicyExistsAsync(int cancellationPolicyId)
        {
            try
            {
                await _cancellationPolicyRepository.GetByIdAsync(cancellationPolicyId);
            }
            catch
            {
                throw new ApiException(new ApiErrorResponse("The cancellation policy does not exist in the database."));
            }
        }
    }
}