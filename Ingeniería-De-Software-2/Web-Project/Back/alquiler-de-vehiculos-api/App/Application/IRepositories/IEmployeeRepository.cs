using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task DeleteByIdAsync(int id);
        Task<List<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
    }
}