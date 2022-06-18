using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IApiClientUser
{
    Task<string> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken);
    Task<string> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken);
    Task<UserGetDto> GetAsync(string token, CancellationToken cancellationToken);
}