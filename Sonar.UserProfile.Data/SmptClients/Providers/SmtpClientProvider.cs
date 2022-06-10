using System.Net.Mail;
using Sonar.UserProfile.Core.Domain.SmtpClients.Providers;
using Microsoft.Extensions.Configuration;

namespace Sonar.UserProfile.Data.SmptClients.Providers;

public class SmtpClientProvider : ISmtpClientProvider
{
    private readonly SmtpClient _smtpClient;

    public SmtpClientProvider(IConfiguration configuration)
    {
        // TODO: add smtpClient configuration 
        _smtpClient = new SmtpClient(configuration["SmtpHost"]);
    }
    
    public void SendEmailAsync(MailAddress mailAddress, MailMessage mailMessage)
    {
        throw new NotImplementedException();
    }
}