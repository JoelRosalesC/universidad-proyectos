using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Branch> _dbSet;

        public BranchRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<Branch>();
        }

        public async Task<Branch> AddAsync(Branch branch)
        {
            _dbSet.Add(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task<Branch> UpdateAsync(Branch branch)
        {
            _dbSet.Update(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var branch = await GetByIdAsync(id);
            _context.Remove(branch);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Branch>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Branch> GetByIdAsync(int id)
        {
            var branch = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if(branch == null)
                throw new ApiException(new ApiErrorResponse("The branch is not already registered in the database."));
            return branch;
        }
    }
}