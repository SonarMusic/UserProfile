namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class TokenNotFoundException : Exception
{
    public TokenNotFoundException()
    {
    }
    
    public TokenNotFoundException(string message) 
        : base(message)
    {
    }

    public TokenNotFoundException(string message, Exception e)
        : base(message, e)
    {
    }
}