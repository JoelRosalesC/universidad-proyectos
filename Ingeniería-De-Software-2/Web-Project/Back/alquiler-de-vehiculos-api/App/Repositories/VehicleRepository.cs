
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Vehicle> _dbSet;

        public VehicleRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<Vehicle>();
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            _dbSet.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }
        public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
        {
            _dbSet.Update(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }
        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<Vehicle> GetByLicensePlateAsync(string licensePlate)
        {
            var vehicle = await _dbSet.FirstOrDefaultAsync(d => d.LicensePlate == licensePlate);
            if(vehicle == null)
                throw new ApiException(new ApiErrorResponse("The vehicle is not already registered in the database."));
            return vehicle;
        }
        public async Task<Vehicle> GetByIdAsync(int id)
        {
            var vehicle = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if(vehicle == null)
                throw new ApiException(new ApiErrorResponse("The vehicle is not already registered in the database."));
            return vehicle;
        }
        public async Task DeleteByIdAsync(int id)
        {
            var vehicle = await GetByIdAsync(id);
            _context.Remove(vehicle);
            await _context.SaveChangesAsync();
        }
    }
}