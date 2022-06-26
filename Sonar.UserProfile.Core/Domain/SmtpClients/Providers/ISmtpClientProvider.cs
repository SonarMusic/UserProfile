using System.Net.Mail;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Providers;

public interface ISmtpClientProvider
{
    Task SendEmailAsync(MailMessage mailMessage);
}