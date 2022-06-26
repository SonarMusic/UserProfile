using System.Net.Mail;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Services;

public interface ISmtpClientService
{
    Task<MailMessage> CreateMailMessageAsync(string email, string subject, string body, CancellationToken cancellationToken);
    void SendMailMessageAsync(MailMessage mailMessage, string userState, CancellationToken cancellationToken);
}