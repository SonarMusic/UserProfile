namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class EmailOccupiedException : Exception
{
    public EmailOccupiedException()
    {
    }
    
    public EmailOccupiedException(string message) 
        : base(message)
    {
    }

    public EmailOccupiedException(string message, Exception e)
        : base(message, e)
    {
    }
}