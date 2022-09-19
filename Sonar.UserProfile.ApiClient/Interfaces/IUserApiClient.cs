using Sonar.UserProfile.ApiClient.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IUserApiClient
{
    /// <summary>
    /// Return a user model if token hasn't expired yet.
    /// </summary>
    /// <param name="token">User token.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>User model which contains: ID, email, AccountType.</returns>
    Task<UserDto> GetAsync(string token, CancellationToken cancellationToken);
}