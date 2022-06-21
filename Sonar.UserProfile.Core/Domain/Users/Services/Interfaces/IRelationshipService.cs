using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

namespace Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;

public interface IRelationshipService
{
    Task SendFriendshipRequestAsync(Guid userId, string targetUserEmail, CancellationToken cancellationToken);

    Task<IReadOnlyList<User>> GetRelationshipsAsync(
        Guid userId, 
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);

    Task AcceptFriendshipRequestAsync(Guid userId, string requestedEmail, CancellationToken cancellationToken);
    Task RejectFriendshipRequestAsync(Guid userId, string requestedEmail, CancellationToken cancellationToken);
}