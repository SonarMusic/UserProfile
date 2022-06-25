using Sonar.UserProfile.ApiClient.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IUserApiClient
{
    Task<string> RegisterAsync(UserAuthDto userAuthDto, CancellationToken cancellationToken);
    Task<string> LoginAsync(UserAuthDto userAuthDto, CancellationToken cancellationToken);
    Task<UserDto> GetAsync(string token, CancellationToken cancellationToken);
}