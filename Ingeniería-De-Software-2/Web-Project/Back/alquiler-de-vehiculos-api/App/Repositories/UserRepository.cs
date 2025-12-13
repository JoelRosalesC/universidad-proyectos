using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public async Task<User> AddAsync(User user)
        {
            _dbSet.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dbSet.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var user = await GetByIdAsync(id);
            _context.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbSet.Where(u => u.Status == UserStatus.Active).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Id == id && u.Status == UserStatus.Active);
            if(user == null)
                throw new ApiException(new ApiErrorResponse("The user is not already registered in the database."));
            return user;
        }

        public async Task<User> GetByMailAsync(string mail)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Mail == mail && u.Status == UserStatus.Active);
            if(user == null)
                throw new ApiException(new ApiErrorResponse("The user is not already registered in the database."));
            return user;
        }
    }
}