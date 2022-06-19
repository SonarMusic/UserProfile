namespace Sonar.UserProfile.Core.Domain.Exceptions;

public class DataOccupiedException : Exception
{
    public DataOccupiedException()
    {
    }
    
    public DataOccupiedException(string message) 
        : base(message)
    {
    }

    public DataOccupiedException(string message, Exception e)
        : base(message, e)
    {
    }
}