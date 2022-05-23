namespace Sonar.UserProfile.Core.Domain.Users.Services;

public interface IUserService
{
    Task<User> GetByIdAsync(Guid tokenId, CancellationToken cancellationToken);
    Task<Guid> RegisterAsync(User user, CancellationToken cancellationToken);
    Task<Guid> LoginAsync(User user, CancellationToken cancellationToken);
    Task Logout(Guid tokenId, CancellationToken cancellationToken);
}