using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Admin> _dbSet;

        public AdminRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<Admin>();
        }

        public async Task<Admin> AddAsync(Admin admin)
        {
            _dbSet.Add(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<Admin> UpdateAsync(Admin admin)
        {
            _dbSet.Update(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var admin = await GetByIdAsync(id);
            _context.Remove(admin);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Admin>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Admin> GetByIdAsync(int id)
        {
            var admin = await _dbSet.FirstOrDefaultAsync(d => d.UserId == id);
            if(admin == null)
                throw new ApiException(new ApiErrorResponse("The admin is not already registered in the database."));
            return admin;
        }
    }
}