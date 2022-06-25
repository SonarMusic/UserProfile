using System.Net.Mail;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Services;

public interface ISmtpClientService
{
    void SendEmailAsync(string email, string subject, string body);
}