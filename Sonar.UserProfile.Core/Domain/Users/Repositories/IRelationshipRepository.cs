namespace Sonar.UserProfile.Core.Domain.Users.Repositories;

public interface IRelationshipRepository
{
    Task SendFriendshipRequestAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetFriendsByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetRequestedByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetRejectedByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsFriendsAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task<bool> IsRequestedAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task<bool> IsRejectedAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
}