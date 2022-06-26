namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class SmtpClientException : Exception
{
    public SmtpClientException()
    {
    }

    public SmtpClientException(string message)
        : base(message)
    {
    }

    public SmtpClientException(string message, Exception e)
        : base(message, e)
    {
    }
}