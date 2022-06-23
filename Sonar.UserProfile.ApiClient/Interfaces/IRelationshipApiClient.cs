using Sonar.UserProfile.ApiClient.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IRelationshipApiClient
{
    Task SendFriendshipRequestAsync(string token, string targetUserEmail, CancellationToken cancellationToken);
    Task<IReadOnlyList<UserDto>> GetFriendsAsync(string token, CancellationToken cancellationToken);
    Task<IReadOnlyList<UserDto>> GetRequestsFromMe(string token, CancellationToken cancellationToken);
    public Task<IReadOnlyList<UserDto>> GetRequestsToMe(string token, CancellationToken cancellationToken);
    Task AcceptFriendshipRequestAsync(string token, string requestedEmail, CancellationToken cancellationToken);
    Task RejectFriendshipRequestAsync(string token, string requestedEmail, CancellationToken cancellationToken);
}