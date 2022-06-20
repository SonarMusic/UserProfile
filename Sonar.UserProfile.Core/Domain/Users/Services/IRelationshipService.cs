namespace Sonar.UserProfile.Core.Domain.Users.Services;

public interface IRelationshipService
{
    Task AddFriend(Guid userId, string friendEmail, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetFriendsById(Guid userId, CancellationToken cancellationToken);
}