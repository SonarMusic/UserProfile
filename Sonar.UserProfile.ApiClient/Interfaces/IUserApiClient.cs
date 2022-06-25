using Sonar.UserProfile.ApiClient.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IUserApiClient
{
    Task<UserDto> GetAsync(string token, CancellationToken cancellationToken);
}