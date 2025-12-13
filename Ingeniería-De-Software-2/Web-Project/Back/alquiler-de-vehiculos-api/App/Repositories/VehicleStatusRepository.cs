using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class VehicleStatusRepository : IVehicleStatusRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<VehicleStatus> _dbSet;

        public VehicleStatusRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<VehicleStatus>();
        }

        public async Task<VehicleStatus> AddAsync(VehicleStatus vehicleStatus)
        {
            _dbSet.Add(vehicleStatus);
            await _context.SaveChangesAsync();
            return vehicleStatus;
        }

        public async Task<VehicleStatus> UpdateAsync(VehicleStatus vehicleStatus)
        {
            _dbSet.Update(vehicleStatus);
            await _context.SaveChangesAsync();
            return vehicleStatus;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var vehicleStatus = await GetByIdAsync(id);
            _context.Remove(vehicleStatus);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VehicleStatus>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<VehicleStatus> GetByIdAsync(int id)
        {
            var vehicleStatus = await _dbSet.FirstOrDefaultAsync(vs => vs.Id == id);
            if(vehicleStatus == null)
                throw new ApiException(new ApiErrorResponse("The vehicle status is not already registered in the database."));
            return vehicleStatus;
        }
    }
}