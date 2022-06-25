using Sonar.UserProfile.ApiClient.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IRelationshipApiClient
{
    Task<IReadOnlyList<UserDto>> GetFriendsAsync(string token, CancellationToken cancellationToken);
    public Task<bool> IsFriends(string token, Guid friendId, CancellationToken cancellationToken);
}