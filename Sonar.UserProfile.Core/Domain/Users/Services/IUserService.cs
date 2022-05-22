namespace Sonar.UserProfile.Core.Domain.Users.Services;

public interface IUserService
{
    Task<User> GetByIdAsync(string stringToken, CancellationToken cancellationToken);
    Task<string> RegisterAsync(User user, CancellationToken cancellationToken);
    Task<string> LoginAsync(User user, CancellationToken cancellationToken);
    string Logout(User user, CancellationToken cancellationToken);
}