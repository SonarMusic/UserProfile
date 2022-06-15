using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient;

public interface IApiClient
{
    Task<string> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken);
    Task<string> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken);
    Task<UserGetDto> GetAsync(string token, CancellationToken cancellationToken);
}