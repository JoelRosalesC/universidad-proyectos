using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class CancellationPolicyRepository : ICancellationPolicyRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<CancellationPolicy> _dbSet;

        public CancellationPolicyRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<CancellationPolicy>();
        }

        public async Task<CancellationPolicy> AddAsync(CancellationPolicy cancellationPolicy)
        {
            _dbSet.Add(cancellationPolicy);
            await _context.SaveChangesAsync();
            return cancellationPolicy;
        }

        public async Task<CancellationPolicy> UpdateAsync(CancellationPolicy cancellationPolicy)
        {
            _dbSet.Update(cancellationPolicy);
            await _context.SaveChangesAsync();
            return cancellationPolicy;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var cancellationPolicy = await GetByIdAsync(id);
            _context.Remove(cancellationPolicy);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CancellationPolicy>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<CancellationPolicy> GetByIdAsync(int id)
        {
            var cancellationPolicy = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if(cancellationPolicy == null)
                throw new ApiException(new ApiErrorResponse("The cancellation policy is not already registered in the database."));
            return cancellationPolicy;
        }
    }
}