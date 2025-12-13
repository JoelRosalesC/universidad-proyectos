using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByMailAsync(string mail);
    }
}