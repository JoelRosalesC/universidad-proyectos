using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Customer> _dbSet;

        public CustomerRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<Customer>();
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            _dbSet.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            _dbSet.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var customer = await GetByIdAsync(id);
            _context.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var customer = await _dbSet.FirstOrDefaultAsync(d => d.UserId == id);
            if(customer == null)
                throw new ApiException(new ApiErrorResponse("The customer is not already registered in the database."));
            return customer;
        }
    }
}