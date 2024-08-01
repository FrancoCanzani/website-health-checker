using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace HealthChecker.Services
{
    public interface IEmailService
    {
        void Send(string receiver, string subject, string body);
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

        public void Send(string receiver, string subject, string body)
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
                    client.Connect(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate(_smtpUser, _smtpPass);
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    Console.WriteLine($"Exception caught in Send(): {ex}");
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
