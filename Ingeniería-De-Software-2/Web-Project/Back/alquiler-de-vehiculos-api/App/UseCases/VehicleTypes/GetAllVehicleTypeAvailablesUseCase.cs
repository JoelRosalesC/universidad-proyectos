using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases.VehicleTypes
{
    public class GetAllVehicleTypeAvailablesUseCase
    {
        private IVehicleTypeRepository _vehicleTypeRepository;
        private IRentalRepository _rentalRepository;
        private IVehicleRepository _vehicleRepository;

        public GetAllVehicleTypeAvailablesUseCase(IVehicleTypeRepository vehicleTypeRepository, IRentalRepository rentalRepository, IVehicleRepository vehicleRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
            _rentalRepository = rentalRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<List<VehicleType>> runAsync(int branchId, DateTime startDate, DateTime endDate)
        {
            var vehicleTypes = await _vehicleTypeRepository.GetAllAsync();
            var vehicles = await _vehicleRepository.GetAllAsync();
            var rentals = await _rentalRepository.GetAllAsync();
            
            // 1. Vehículos en la sucursal
            var vehiclesInBranch = vehicles
                .Where(v => v.CurrentBranchId == branchId)
                .ToList();

            // 2. Agrupar por tipo
            var result = vehicleTypes
                .Where(vt =>
                {
                    // vehículos de ese tipo en la sucursal
                    var vehiclesOfType = vehiclesInBranch
                        .Where(v => v.VehicleTypeId == vt.Id)
                        .ToList();

                    if (!vehiclesOfType.Any())
                        return false;

                    int totalVehicles = vehiclesOfType.Count;

                    // 3. Buscar cuántas veces fue reservado ese tipo en el rango
                    int activeRentals = rentals
                        .Where(r =>
                            r.SelectedVehicleTypeId == vt.Id &&
                            r.PickedUpBranchId == branchId &&
                            // rango de fechas que se superponen
                            !(r.ReturnDate.AddDays(1) < startDate || r.RentalDate.AddDays(-1) > endDate)
                        )
                        .Count();

                    // 4. Disponible si hay más autos que reservas
                    return activeRentals < totalVehicles;
                })
                .ToList();

            return result;
        }
    }
}