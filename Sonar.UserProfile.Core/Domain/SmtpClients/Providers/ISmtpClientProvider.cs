using System.Net.Mail;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Providers;

public interface ISmtpClientProvider
{
    void SendEmailAsync(MailMessage mailMessage, string userState);
}