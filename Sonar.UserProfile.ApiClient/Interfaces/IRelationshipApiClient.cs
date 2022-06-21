using Sonar.UserProfile.ApiClient.Dto;
using Sonar.UserProfile.ApiClient.ValueObjects;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IRelationshipApiClient
{
    Task SendFriendshipRequestAsync(string token, string targetUserEmail, CancellationToken cancellationToken);

    Task<IReadOnlyList<UserGetDto>> GetRelationshipsAsync(
        string token, 
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken);

    Task AcceptFriendshipRequestAsync(string token, string requestedEmail, CancellationToken cancellationToken);
    Task RejectFriendshipRequestAsync(string token, string requestedEmail, CancellationToken cancellationToken);
}