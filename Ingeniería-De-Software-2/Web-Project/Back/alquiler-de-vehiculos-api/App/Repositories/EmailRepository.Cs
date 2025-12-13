using System.Net.Mail;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net;
using AlquilerDeVehiculosApi.App.Application.IRepositories;


namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        
        private readonly IConfiguration _configuration;
        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
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