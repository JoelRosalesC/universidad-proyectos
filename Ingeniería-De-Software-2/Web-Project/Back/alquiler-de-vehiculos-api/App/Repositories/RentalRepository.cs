using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Rental> _dbSet;

        public RentalRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<Rental>();
        }

        public async Task<Rental> AddAsync(Rental rental)
        {
            _dbSet.Add(rental);
            await _context.SaveChangesAsync();
            return rental;
        }

        public async Task<Rental> UpdateAsync(Rental rental)
        {
            _dbSet.Update(rental);
            await _context.SaveChangesAsync();
            return rental;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var rental = await GetByIdAsync(id);
            _context.Remove(rental);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Rental>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<List<Rental>> GetAllByCustomerIdAsync(int customerId)
        {
            return await _dbSet.Where(r => r.CustomerId == customerId).ToListAsync();
        }
        public async Task<List<Rental>> GetRentalsInBranch(int branchId)
        {
            return await _dbSet.Where(r => r.PickedUpBranchId == branchId).ToListAsync();
        }

        public async Task<Rental> GetByIdAsync(int id)
        {
            var rental = await _dbSet.FirstOrDefaultAsync(d => d.Id == id);
            if (rental == null)
                throw new ApiException(new ApiErrorResponse("The rental is not already registered in the database."));
            return rental;
        }
    }
}