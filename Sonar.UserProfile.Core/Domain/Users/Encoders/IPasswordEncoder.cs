namespace Sonar.UserProfile.Core.Domain.Users.Encoders;

public interface IPasswordEncoder
{
    public string GetRandomSalt();
    public string Encode(string rawPassword);
    public bool Matches(string rawPassword, string hashedPassword);
}