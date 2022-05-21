using Sonar.UserProfile.Core.Domain.ValueObjects;

namespace Sonar.UserProfile.Core.Domain.Users;

public class User
{
    public Guid Id { get; set; }
    public string Password { get; set; }
    public Token Token { get; set; }
}