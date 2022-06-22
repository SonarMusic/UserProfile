using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Core.Domain.Users.Repositories;

public interface IRelationshipRepository
{
    Task SendFriendshipRequestAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken);

    Task<IReadOnlyList<User>> GetRelationshipUsersAsync(
        Guid id,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);

    Task<bool> IsRelationshipAsync(
        Guid leftUserId,
        Guid rightUserId,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);

    Task UpdateStatusAsync(
        Guid leftUserId,
        Guid rightUserId,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);

    Task Delete(Guid leftUserId, Guid rightUserId, CancellationToken cancellationToken);
}