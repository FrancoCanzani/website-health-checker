using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace HealthChecker.Services
{
    public interface IEmailService
    {
        Task SendAsync(string receiver, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService()
        {

            _smtpHost = "sandbox.smtp.mailtrap.io";
            _smtpPort = 587;
            _smtpUser = "96e6f9c50d1b1d";
            _smtpPass = "96923e7eabdf0c";
        }

        public async Task SendAsync(string receiver, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Health Check Alert", "sender@example.com"));
            message.To.Add(new MailboxAddress("Receiver", receiver));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUser, _smtpPass);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    Console.WriteLine($"Exception caught in Send(): {ex}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
