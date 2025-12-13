using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class MaintenanceRepository : IMaintenanceRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Maintenance> _dbSet;

        public MaintenanceRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<Maintenance>();
        }

        public async Task<Maintenance> AddAsync(Maintenance maintenance)
        {
            _dbSet.Add(maintenance);
            await _context.SaveChangesAsync();
            return maintenance;
        }

        public async Task<Maintenance> UpdateAsync(Maintenance maintenance)
        {
            _dbSet.Update(maintenance);
            await _context.SaveChangesAsync();
            return maintenance;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var maintenance = await GetByIdAsync(id);
            _context.Remove(maintenance);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Maintenance>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Maintenance> GetByIdAsync(int id)
        {
            var maintenance = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if(maintenance == null)
                throw new ApiException(new ApiErrorResponse("The maintenance is not already registered in the database."));
            return maintenance;
        }
    }
}