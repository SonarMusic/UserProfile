namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class InvalidRequestException : Exception
{
    public InvalidRequestException()
    {
    }
    
    public InvalidRequestException(string message) 
        : base(message)
    {
    }

    public InvalidRequestException(string message, Exception e)
        : base(message, e)
    {
    }
}