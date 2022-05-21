namespace Sonar.UserProfile.Core.Domain.Users.Services;

public interface IUserService
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> RegisterAsync(User user, CancellationToken cancellationToken);
    Task LoginAsync(User user, CancellationToken cancellationToken);
    Task LogoutAsync(Guid id, CancellationToken cancellationToken);
}