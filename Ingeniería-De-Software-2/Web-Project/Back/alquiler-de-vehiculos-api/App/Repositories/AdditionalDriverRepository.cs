using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class AdditionalDriverRepository : IAdditionalDriverRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<AdditionalDriver> _dbSet;

        public AdditionalDriverRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<AdditionalDriver>();
        }

        public async Task<AdditionalDriver> AddAsync(AdditionalDriver additionalDriver)
        {
            _dbSet.Add(additionalDriver);
            await _context.SaveChangesAsync();
            return additionalDriver;
        }

        public async Task<AdditionalDriver> UpdateAsync(AdditionalDriver additionalDriver)
        {
            _dbSet.Update(additionalDriver);
            await _context.SaveChangesAsync();
            return additionalDriver;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var additionalDriver = await GetByIdAsync(id);
            _context.Remove(additionalDriver);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AdditionalDriver>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<AdditionalDriver> GetByIdAsync(int id)
        {
            var additionalDriver = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if(additionalDriver == null)
                throw new ApiException(new ApiErrorResponse("The additional driver is not already registered in the database."));
            return additionalDriver;
        }
    }
}