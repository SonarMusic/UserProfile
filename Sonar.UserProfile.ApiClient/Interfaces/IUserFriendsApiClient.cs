using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient.Interfaces;

public interface IUserFriendsApiClient
{
    Task AddFriendAsync(string token, string friendEmail, CancellationToken cancellationToken);
    Task<IReadOnlyList<UserGetDto>> GetFriendsAsync(string token, CancellationToken cancellationToken);
}