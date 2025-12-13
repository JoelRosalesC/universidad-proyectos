using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class AdditionalRentalRepository : IAdditionalRentalRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<AdditionalRental> _dbSet;

        public AdditionalRentalRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<AdditionalRental>();
        }

        public async Task<AdditionalRental> AddAsync(AdditionalRental additionalRental)
        {
            _dbSet.Add(additionalRental);
            await _context.SaveChangesAsync();
            return additionalRental;
        }

        public async Task<AdditionalRental> UpdateAsync(AdditionalRental additionalRental)
        {
            _dbSet.Update(additionalRental);
            await _context.SaveChangesAsync();
            return additionalRental;
        }

        public async Task DeleteAsync(int additionalId, int rentalId)
        {
            var additionalRental = await GetAsync(additionalId, rentalId);
            _context.Remove(additionalRental);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AdditionalRental>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<AdditionalRental> GetAsync(int additionalId, int rentalId)
        {
            var additionalRental = await _dbSet.FirstOrDefaultAsync(d => d.AdditionalId == additionalId && d.RentalId == rentalId);
            if(additionalRental == null)
                throw new ApiException(new ApiErrorResponse("The additional rental is not already registered in the database."));
            return additionalRental;
        }
    }
}