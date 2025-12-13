using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IAdditionalRentalRepository
    {
        Task<AdditionalRental> AddAsync(AdditionalRental additionalRental);
        Task<AdditionalRental> UpdateAsync(AdditionalRental additionalRental);
        Task DeleteAsync(int additionalId, int rentalId);
        Task<List<AdditionalRental>> GetAllAsync();
        Task<AdditionalRental> GetAsync(int additionalId, int rentalId);
    }
}