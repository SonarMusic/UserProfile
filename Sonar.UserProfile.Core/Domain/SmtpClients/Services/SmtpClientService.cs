using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Sonar.UserProfile.Core.Domain.SmtpClients.Providers;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Services;

public class SmtpClientService : ISmtpClientService
{
    private readonly ISmtpClientProvider _smtpClientProvider;

    public SmtpClientService(ISmtpClientProvider smtpClientProvider)
    {
        _smtpClientProvider = smtpClientProvider;
    }

    public void SendEmailsAsync(IEnumerable<string> emails, MailMessage mailMessage)
    {
        foreach (var email in emails)
        {
            try
            {
                var currentMail = new MailAddress(email);
                _smtpClientProvider.SendEmailAsync(currentMail, mailMessage);
            }
            catch (Exception invalidEmailException)
            {
                //somethingDI.log($'user with email={email} has invalid email');
            }
        }
    }
}