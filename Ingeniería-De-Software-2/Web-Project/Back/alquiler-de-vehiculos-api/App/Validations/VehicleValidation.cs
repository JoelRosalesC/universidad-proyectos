
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.Validations
{
    public class VehicleValidation
    {
        private IVehicleRepository _vehicleRepository;
        private IRentalRepository _rentalRepository;

        public VehicleValidation(IVehicleRepository vehicleRepository, IRentalRepository rentalRepository)
        {
            _vehicleRepository = vehicleRepository;
            _rentalRepository = rentalRepository;
        }
        public async Task LicensePlateAlreadyExistsAsync(string licensePlate)
        {
            try
            {
                await _vehicleRepository.GetByLicensePlateAsync(licensePlate);
            }
            catch
            {
                return;
            }
            throw new ApiException(new ApiErrorResponse("La patente ya está registrada."));
        }
        public async Task LicensePlateAlreadyExistsOrIdEqualsAsync(string licensePlate, int id)
        {
            try
            {
                var v = await _vehicleRepository.GetByLicensePlateAsync(licensePlate);
                if (v.Id == id)
                    return;
            }
            catch
            {
                return;
            }
            throw new ApiException(new ApiErrorResponse("La patente ya está registrada."));
        }
        public async Task NotExistingAsync(int id)
        {
            try
            {
                await _vehicleRepository.GetByIdAsync(id);
            }
            catch (ApiException)
            {

                throw new ApiException(new ApiErrorResponse("el vehiculo no existe"));
            }
        }
        public async Task RentedVehicleAsync(int id)
        {
            var rentals = await _rentalRepository.GetAllAsync();
            if (rentals.Where(r => r.DeliveredVehicleId == id && r.Status == RentalStatus.Rented).Count() > 0)
            {
                throw new ApiException(new ApiErrorResponse("el vehiculo se encuentra alquilado en este momento"));
            }
        }
        
    }
}