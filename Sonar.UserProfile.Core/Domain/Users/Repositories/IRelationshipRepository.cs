namespace Sonar.UserProfile.Core.Domain.Users.Repositories;

public interface IRelationshipRepository
{
    Task SendFriendshipRequestAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetFriendsAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetRequestedAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetRejectedAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsFriendsAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task<bool> IsRequestedAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task<bool> IsRejectedAsync(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task AcceptFriendshipRequestAsync(Guid userId, Guid requestedId, CancellationToken cancellationToken);
    Task RejectFriendshipRequestAsync(Guid userId, Guid requestedId, CancellationToken cancellationToken);
}