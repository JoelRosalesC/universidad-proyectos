using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.Application.Validations
{
    public class BranchValidation
    {
        private IBranchRepository _branchRepository;
        private IVehicleRepository _vehicleRepository;
        private IRentalRepository _rentalRepository;

        public BranchValidation(IBranchRepository branchRepository, IVehicleRepository vehicleRepository, IRentalRepository rentalRepository)
        {
            _branchRepository = branchRepository;
            _vehicleRepository = vehicleRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task BranchExistsAsync(int id)
        {
            try
            {
                await _branchRepository.GetByIdAsync(id);
            }
            catch
            {
                throw new ApiException(new ApiErrorResponse("La sucursal no existe en la base de datos."));
            }
        }

        public async Task BranchNameAlreadyExistsAsync(string name)
        {
            var branches = await _branchRepository.GetAllAsync();
            var existingBranch = branches.FirstOrDefault(b => b.Name.ToLower() == name.ToLower());

            if (existingBranch != null)
            {
                throw new ApiException(new ApiErrorResponse("Ya existe una sucursal con este nombre."));
            }
        }
        public async Task BranchNameAlreadyExistsOrIdEqualsAsync(string name, int id)
        {
            var branches = await _branchRepository.GetAllAsync();
            var existingBranch = branches.FirstOrDefault(b => b.Name.ToLower() == name.ToLower());

            if (existingBranch != null && existingBranch.Id != id)
            {
                throw new ApiException(new ApiErrorResponse("Ya existe una sucursal con este nombre."));
            }
        }
        public async Task IsReadyToBeDeleted(int id)
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            var rentals = await _rentalRepository.GetAllAsync();
            if (vehicles.Where(v => v.CurrentBranchId == id).Count() > 0)
            {
                throw new ApiException(new ApiErrorResponse("La sucursal tiene vehiculos"));
            }
            if (rentals.Where(r => r.PickedUpBranchId == id && r.Status == Enums.RentalStatus.Pending).Count() > 0)
            {
                throw new ApiException(new ApiErrorResponse("La sucursal tiene reservas pendientes"));
            }
        }
    }
}