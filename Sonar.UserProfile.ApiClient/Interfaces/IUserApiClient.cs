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
    
    /// <summary>
    /// Generate new user token to discord bot. Token will expire in 7 days.
    /// </summary>
    /// <param name="discordBotToken">Token of sonar discord bot.</param>
    /// <param name="userDiscordId">Discord id of target user.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>New user token.</returns>
    Task<string> LoginByDiscordBotAsync(string discordBotToken, string userDiscordId, CancellationToken cancellationToken);
}