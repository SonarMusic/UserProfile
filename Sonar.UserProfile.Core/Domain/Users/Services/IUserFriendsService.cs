namespace Sonar.UserProfile.Core.Domain.Users.Services;

public interface IUserFriendsService
{
    Task AddFriend(Guid userId, string friendEmail, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetFriendsById(Guid userId, CancellationToken cancellationToken);
}