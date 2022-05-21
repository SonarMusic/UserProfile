namespace Sonar.UserProfile.Core.Domain.ValueObjects;

public class Token
{
    public Guid Id { get; set; }
    public DateTime ExpirationDate { get; set; }
}