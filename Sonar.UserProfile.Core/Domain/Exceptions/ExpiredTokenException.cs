namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class ExpiredTokenException : Exception
{
    public ExpiredTokenException()
    {
    }
    
    public ExpiredTokenException(string message) 
        : base(message)
    {
    }

    public ExpiredTokenException(string message, Exception e)
        : base(message, e)
    {
    }
}