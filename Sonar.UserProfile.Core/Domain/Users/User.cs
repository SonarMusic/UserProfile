namespace Sonar.UserProfile.Core.Domain.Users;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<User> Friends { get; set; }
}