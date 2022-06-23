using Sonar.UserProfile.ApiClient.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IUserApiClient
{
    Task<string> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken);
    Task<string> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken);
    Task<UserDto> GetAsync(string token, CancellationToken cancellationToken);
}