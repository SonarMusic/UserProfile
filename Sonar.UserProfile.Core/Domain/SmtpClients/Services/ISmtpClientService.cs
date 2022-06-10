using System.Net.Mail;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Services;

public interface ISmtpClientService
{
    void SendEmailsAsync(IEnumerable<string> emails, MailMessage mailMessage);
}