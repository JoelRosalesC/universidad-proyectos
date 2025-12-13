
using AlquilerDeVehiculosApi.App.Application.Entities.Models;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IEmailQueueRepository
    {
        Task EnqueueAsync(string toEmail, string subject, string body);
        Task<EmailMessage> DequeueAsync(CancellationToken cancellationToken);
    }
}