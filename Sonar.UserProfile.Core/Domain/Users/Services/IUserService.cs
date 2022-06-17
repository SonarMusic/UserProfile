namespace Sonar.UserProfile.Core.Domain.Users.Services;

public interface IUserService
{
    Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<string> RegisterAsync(User user, CancellationToken cancellationToken);
    Task<string> LoginAsync(User user, CancellationToken cancellationToken);
    Task AddFriend(Guid userId, string friendEmail, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetFriendsById(Guid userId, CancellationToken cancellationToken);
}