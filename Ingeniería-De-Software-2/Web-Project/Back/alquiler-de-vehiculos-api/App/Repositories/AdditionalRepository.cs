using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class AdditionalRepository : IAdditionalRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Additional> _dbSet;

        public AdditionalRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<Additional>();
        }

        public async Task<Additional> AddAsync(Additional additional)
        {
            _dbSet.Add(additional);
            await _context.SaveChangesAsync();
            return additional;
        }

        public async Task<Additional> UpdateAsync(Additional additional)
        {
            _dbSet.Update(additional);
            await _context.SaveChangesAsync();
            return additional;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var additional = await GetByIdAsync(id);
            _context.Remove(additional);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Additional>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Additional> GetByIdAsync(int id)
        {
            var additional = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if(additional == null)
                throw new ApiException(new ApiErrorResponse("The additional is not already registered in the database."));
            return additional;
        }
    }
}