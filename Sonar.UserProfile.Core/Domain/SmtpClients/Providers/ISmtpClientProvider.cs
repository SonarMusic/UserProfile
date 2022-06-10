using System.Net.Mail;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Providers;

public interface ISmtpClientProvider
{
    void SendEmailAsync(MailAddress mailAddress, MailMessage mailMessage);
}