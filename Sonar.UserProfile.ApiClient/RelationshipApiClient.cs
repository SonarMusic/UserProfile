using System.Net;
using System.Text.Json;
using Sonar.UserProfile.ApiClient.Interfaces;
using Sonar.UserProfile.ApiClient.Tools;
using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient;

public class RelationshipApiClient : IRelationshipApiClient
{
    private readonly HttpClient _httpClient;
    private readonly RequestCreator _requestCreator;

    public RelationshipApiClient(string baseUrl, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _requestCreator = new RequestCreator(baseUrl);
    }

    /// <summary>
    /// Add a friend to user if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user..</param>
    /// <param name="friendEmail">An email if friend who you want to add.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    public async Task AddFriendAsync(string token, string friendEmail, CancellationToken cancellationToken)
    {
        var request =
            _requestCreator.RequestWithContentAndToken("/relationship/add-friend", "POST", token, friendEmail);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return;
        }

        var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
        throw new ApiClientException(errorMessage);
    }

    /// <summary>
    /// Return a friend list if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of user's friends. Every friend is UserGetDto which contains: Id, Email.</returns>
    public async Task<IReadOnlyList<UserGetDto>> GetFriendsAsync(string token, CancellationToken cancellationToken)
    {
        var request = _requestCreator.RequestWithToken("/relationship/get-friends", "GET", token);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
            throw new ApiClientException(errorMessage);
        }

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseDeserialized = JsonSerializer.Deserialize<IReadOnlyList<UserGetDto>>(responseString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        return responseDeserialized!;
    }
}