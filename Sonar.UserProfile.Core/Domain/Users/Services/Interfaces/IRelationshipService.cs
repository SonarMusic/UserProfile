namespace Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;

public interface IRelationshipService
{
    Task AddFriendAsync(Guid userId, string friendEmail, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetFriendsAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetRequestedAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetRejectedAsync(Guid userId, CancellationToken cancellationToken);
    Task AcceptFriendshipRequestAsync(Guid userId, Guid requestedId, CancellationToken cancellationToken);
    Task RejectFriendshipRequestAsync(Guid userId, Guid requestedId, CancellationToken cancellationToken);
}