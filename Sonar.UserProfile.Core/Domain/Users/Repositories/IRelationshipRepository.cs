using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Core.Domain.Users.Repositories;

public interface IRelationshipRepository
{
    Task SendFriendshipRequestAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken);

    Task<IReadOnlyList<User>> GetRelationshipsAsync(
        Guid id,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);

    Task<bool> IsRelationshipAsync(
        Guid userId,
        Guid friendId,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);

    Task ChangeRelationshipStatusAsync(
        Guid userId, 
        Guid requestedId, 
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);
}