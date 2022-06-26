using System.Net.Mail;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Services;

public interface ISmtpClientService
{
    MailMessage CreateMailMessageAsync(string email, string subject, string body);
}