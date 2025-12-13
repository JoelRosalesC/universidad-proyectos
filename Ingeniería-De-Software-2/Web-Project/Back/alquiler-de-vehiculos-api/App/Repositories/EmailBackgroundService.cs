using System.Threading.Channels;
using AlquilerDeVehiculosApi.App.Application.Entities.Models;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using MailKit.Security;
using MimeKit;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IEmailQueueRepository _emailQueue;
        private readonly IConfiguration _configuration;

        public EmailBackgroundService(IEmailQueueRepository emailQueue, IConfiguration configuration)
        {
            _emailQueue = emailQueue;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var email = await _emailQueue.DequeueAsync(stoppingToken);
                await EnviarCorreoAsync(email);
            }
        }

        private async Task EnviarCorreoAsync(EmailMessage email)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(email.ToEmail));
            message.Subject = email.Subject;
            message.Body = new TextPart("html")
            {
                Text = email.Body
            };
            using var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando email: {ex.Message}");
                throw;
            }
        }
    }

}