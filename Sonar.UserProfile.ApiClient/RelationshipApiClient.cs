using System.Net;
using System.Text.Json;
using Sonar.UserProfile.ApiClient.Dto;
using Sonar.UserProfile.ApiClient.Interfaces;
using Sonar.UserProfile.ApiClient.Tools;

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
    /// Return list of user's friends if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of users. Every user is UserDto which contains: Id, Email.</returns>
    public async Task<IReadOnlyList<UserDto>> GetFriendsAsync(string token, CancellationToken cancellationToken)
    {
        var request =
            _requestCreator.RequestWithToken(
                "/relationship/get-friends",
                "GET",
                token);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode is not HttpStatusCode.OK)
        {
            var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
            throw new ApiClientException(errorMessage);
        }

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseDeserialized = JsonSerializer.Deserialize<IReadOnlyList<UserDto>>(responseString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        return responseDeserialized!;
    }

    /// <summary>
    /// Get bool statement if users are friends.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="friendId">Id of user, friendship with who you want to check.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>True if users are friends and false if opposite.</returns>
    public async Task<bool> IsFriends(string token, Guid friendId, CancellationToken cancellationToken)
    {
        var request = _requestCreator.RequestWithToken(
            $"/relationship/is-friends?friendId={friendId}",
            "GET",
            token);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode is not HttpStatusCode.OK)
        {
            var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
            throw new ApiClientException(errorMessage);
        }

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseDeserialized = JsonSerializer.Deserialize<bool>(responseString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return responseDeserialized;
    }
}