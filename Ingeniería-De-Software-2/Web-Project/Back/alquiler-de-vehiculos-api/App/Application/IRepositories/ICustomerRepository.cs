using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteByIdAsync(int id);
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
    }
}