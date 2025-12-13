
using AlquilerDeVehiculosApi.App.Application.Entities;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate> AddAsync(EmailTemplate emailTemplate);
        Task<EmailTemplate> UpdateAsync(EmailTemplate emailTemplate);
        Task DeleteByIdAsync(int id);
        Task<EmailTemplate> GetByIdAsync(int id);
        Task<EmailTemplate> GetByNameAsync(string name);
        Task<List<EmailTemplate>> GetAllAsync();
    }
}