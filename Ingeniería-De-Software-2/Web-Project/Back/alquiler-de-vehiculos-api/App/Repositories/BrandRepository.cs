using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Brand> _dbSet;

        public BrandRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<Brand>();
        }

        public async Task<Brand> AddAsync(Brand brand)
        {
            _dbSet.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand> UpdateAsync(Brand brand)
        {
            _dbSet.Update(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var brand = await GetByIdAsync(id);
            _context.Remove(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            var brand = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if(brand == null)
                throw new ApiException(new ApiErrorResponse("The brand is not already registered in the database."));
            return brand;
        }
    }
}