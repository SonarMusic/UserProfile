using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Core.Domain.Users.Repositories;

public interface IRelationshipRepository
{
    Task SendFriendshipRequestAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken);

    Task<IReadOnlyList<User>> GetUsersInRelationshipFromUserAsync(
        Guid id,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);
    
    Task<IReadOnlyList<User>> GetUsersInRelationshipToUserAsync(
        Guid id,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);

    Task<RelationshipStatus> GetStatusAsync(
        Guid senderUserId,
        Guid targetUserId,
        CancellationToken cancellationToken);

    Task UpdateStatusAsync(
        Guid senderUserId,
        Guid targetUserId,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);

    Task DeleteAsync(Guid senderUserId, Guid targetUserId, CancellationToken cancellationToken);
}