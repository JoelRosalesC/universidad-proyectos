using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class VehicleMaintenanceRepository : IVehicleMaintenanceRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<VehicleMaintenance> _dbSet;

        public VehicleMaintenanceRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<VehicleMaintenance>();
        }

        public async Task<VehicleMaintenance> AddAsync(VehicleMaintenance vehicleMaintenance)
        {
            _dbSet.Add(vehicleMaintenance);
            await _context.SaveChangesAsync();
            return vehicleMaintenance;
        }

        public async Task<VehicleMaintenance> UpdateAsync(VehicleMaintenance vehicleMaintenance)
        {
            _dbSet.Update(vehicleMaintenance);
            await _context.SaveChangesAsync();
            return vehicleMaintenance;
        }

        public async Task DeleteByIdAsync(int vehicleId, int maintenanceId)
        {
            var vehicleMaintenance = await GetByIdAsync(vehicleId, maintenanceId);
            _context.Remove(vehicleMaintenance);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VehicleMaintenance>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<VehicleMaintenance> GetByIdAsync(int vehicleId, int maintenanceId)
        {
            var vehicleMaintenance = await _dbSet.FirstOrDefaultAsync(vm => 
                vm.VehicleId == vehicleId && vm.MaintenanceId == maintenanceId);
            
            if(vehicleMaintenance == null)
                throw new ApiException(new ApiErrorResponse("The vehicle maintenance is not already registered in the database."));
            
            return vehicleMaintenance;
        }
    }
}