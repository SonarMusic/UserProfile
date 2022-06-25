﻿using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.SmtpClients.Providers;

namespace Sonar.UserProfile.Core.Domain.SmtpClients.Services;

public class SmtpClientService : ISmtpClientService
{
    private readonly ISmtpClientProvider _smtpClientProvider;
    private readonly IConfiguration _configuration;

    public SmtpClientService(ISmtpClientProvider smtpClientProvider, IConfiguration configuration)
    {
        _smtpClientProvider = smtpClientProvider;
        _configuration = configuration;
    }

    public void SendEmailAsync(string email, string subject, string body)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["NoReplyEmail"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);
        _smtpClientProvider.SendEmailAsync(mailMessage, "smtp.gmail.com");
    }
}