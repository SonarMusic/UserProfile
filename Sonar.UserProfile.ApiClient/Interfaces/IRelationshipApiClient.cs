using Sonar.UserProfile.ApiClient.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IRelationshipApiClient
{
    /// <summary>
    /// Return list of user's friends if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of users. Every user is UserDto which contains: Id, Email.</returns>
    Task<IReadOnlyList<UserDto>> GetFriendsAsync(string token, CancellationToken cancellationToken);
    
    /// <summary>
    /// Get bool statement if users are friends.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="friendId">Id of user, friendship with who you want to check.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>True if users are friends and false if opposite.</returns>
    public Task<bool> IsFriends(string token, Guid friendId, CancellationToken cancellationToken);
}