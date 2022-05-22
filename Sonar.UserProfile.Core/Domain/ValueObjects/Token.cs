using Sonar.UserProfile.Core.Domain.Users;

namespace Sonar.UserProfile.Core.Domain.ValueObjects;

public class Token
{
    public User User { get; set; }
    public DateTime ExpirationDate { get; set; }
}