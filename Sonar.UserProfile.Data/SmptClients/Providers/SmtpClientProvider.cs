using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using Sonar.UserProfile.Core.Domain.SmtpClients.Providers;
using Microsoft.Extensions.Configuration;

namespace Sonar.UserProfile.Data.SmptClients.Providers;

public class SmtpClientProvider : ISmtpClientProvider
{
    private readonly SmtpClient _smtpClient;
    private static bool _isMailSent;

    public SmtpClientProvider(IConfiguration configuration)
    {
        _smtpClient = new SmtpClient(configuration["SmtpHost"], Convert.ToInt32(configuration["SmtpPort"]));
        _smtpClient.Credentials = new NetworkCredential(configuration["SmtpNoReplyMail"], configuration["SmtpNoReplyMailPassword"]);
        _smtpClient.EnableSsl = true;
    }

    public async Task SendEmailAsync(MailMessage mailMessage)
    {
        _smtpClient.Send(mailMessage);
    }
}