using Sonar.UserProfile.ApiClient.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IRelationshipApiClient
{
    Task SendFriendshipRequestAsync(string token, string targetUserEmail, CancellationToken cancellationToken);

    Task<IReadOnlyList<UserGetDto>> GetFriendsAsync(string token, CancellationToken cancellationToken);

    Task AcceptFriendshipRequestAsync(string token, string requestedEmail, CancellationToken cancellationToken);
    Task RejectFriendshipRequestAsync(string token, string requestedEmail, CancellationToken cancellationToken);
}