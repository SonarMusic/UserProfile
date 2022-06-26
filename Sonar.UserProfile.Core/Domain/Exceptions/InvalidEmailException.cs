namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class InvalidEmailException : Exception
{
    public InvalidEmailException()
    {
    }
    
    public InvalidEmailException(string message) 
        : base(message)
    {
    }

    public InvalidEmailException(string message, Exception e)
        : base(message, e)
    {
    }
}