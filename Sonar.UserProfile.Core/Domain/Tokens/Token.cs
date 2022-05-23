namespace Sonar.UserProfile.Core.Domain.Tokens;

public class Token
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
}