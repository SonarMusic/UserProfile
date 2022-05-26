namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class IAmTeapotException : Exception
{
    public IAmTeapotException()
    {
    }
    
    public IAmTeapotException(string message) 
        : base(message)
    {
    }

    public IAmTeapotException(string message, Exception e)
        : base(message, e)
    {
    }
}