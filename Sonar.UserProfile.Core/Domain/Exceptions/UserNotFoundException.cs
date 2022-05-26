namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException()
    {
    }
    
    public UserNotFoundException(string message) 
        : base(message)
    {
    }

    public UserNotFoundException(string message, Exception e)
        : base(message, e)
    {
    }
}