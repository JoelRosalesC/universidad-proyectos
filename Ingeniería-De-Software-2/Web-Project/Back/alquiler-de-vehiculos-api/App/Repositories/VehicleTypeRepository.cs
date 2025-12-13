using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<VehicleType> _dbSet;

        public VehicleTypeRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<VehicleType>();
        }

        public async Task<VehicleType> AddAsync(VehicleType vehicleType)
        {
            _dbSet.Add(vehicleType);
            await _context.SaveChangesAsync();
            return vehicleType;
        }

        public async Task<VehicleType> UpdateAsync(VehicleType vehicleType)
        {
            _dbSet.Update(vehicleType);
            await _context.SaveChangesAsync();
            return vehicleType;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var vehicleType = await GetByIdAsync(id);
            _context.Remove(vehicleType);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VehicleType>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<VehicleType> GetByIdAsync(int id)
        {
            var vehicleType = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if(vehicleType == null)
                throw new ApiException(new ApiErrorResponse("The vehicle type is not already registered in the database."));
            return vehicleType;
        }
    }
}