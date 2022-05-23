namespace Sonar.UserProfile.Data.Tokens;

public class TokenDbModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
}