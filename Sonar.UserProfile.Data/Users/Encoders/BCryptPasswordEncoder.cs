using Sonar.UserProfile.Core.Domain.Users.Encoders;
namespace Sonar.UserProfile.Data.Users.Encoders;

public class BCryptPasswordEncoder : IPasswordEncoder
{
    public string GetRandomSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12);
    }

    public string Encode(string rawPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(rawPassword);

    }

    public bool Matches(string rawPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(rawPassword, hashedPassword);
    }
}