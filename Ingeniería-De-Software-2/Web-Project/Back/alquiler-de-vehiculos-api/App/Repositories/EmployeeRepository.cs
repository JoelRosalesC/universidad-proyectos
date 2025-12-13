using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Employee> _dbSet;

        public EmployeeRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<Employee>();
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            _dbSet.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _dbSet.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            _context.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var employee = await _dbSet.FirstOrDefaultAsync(d => d.UserId == id);
            if(employee == null)
                throw new ApiException(new ApiErrorResponse("The employee is not already registered in the database."));
            return employee;
        }
    }
}