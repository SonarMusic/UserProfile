namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }
    
    public NotFoundException(string message) 
        : base(message)
    {
    }

    public NotFoundException(string message, Exception e)
        : base(message, e)
    {
    }
}