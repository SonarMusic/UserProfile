using System.Net.Mail;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Providers;

public interface ISmtpClientProvider
{
    Task<bool> SendEmailAsync(MailMessage mailMessage, string userState);
}