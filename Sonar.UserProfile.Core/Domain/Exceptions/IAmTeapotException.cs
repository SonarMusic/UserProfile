namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class IAmATeapotException : Exception
{
    public IAmATeapotException()
        : base("Server refuses to brew coffee because it is, permanently, a teapot")
    {
    }
    
    public IAmATeapotException(string message) 
        : base(message)
    {
    }

    public IAmATeapotException(string message, Exception e)
        : base(message, e)
    {
    }
}