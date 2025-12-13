using System.Threading.Channels;
using AlquilerDeVehiculosApi.App.Application.Entities.Models;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class EmailQueueRepository: IEmailQueueRepository
    {
        private readonly Channel<EmailMessage> _queue = Channel.CreateUnbounded<EmailMessage>();

        public async Task EnqueueAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new EmailMessage()
            {
                ToEmail = toEmail,
                Subject = subject,
                Body = body
            };
            await _queue.Writer.WriteAsync(emailMessage);
        }

        public async Task<EmailMessage> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }

}