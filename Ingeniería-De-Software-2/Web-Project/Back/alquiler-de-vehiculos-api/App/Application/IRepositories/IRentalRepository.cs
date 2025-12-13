using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IRentalRepository
    {
        Task<Rental> AddAsync(Rental rental);
        Task<Rental> UpdateAsync(Rental rental);
        Task DeleteByIdAsync(int id);
        Task<List<Rental>> GetAllAsync();
        Task<List<Rental>> GetAllByCustomerIdAsync(int customerId);
        Task<List<Rental>> GetRentalsInBranch(int branchId);
        Task<Rental> GetByIdAsync(int id);
    }
}