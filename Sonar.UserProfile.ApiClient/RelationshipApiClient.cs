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
    /// Send a friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="targetUserEmail">An email of user who you want to send request.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    public async Task SendFriendshipRequestAsync(
        string token,
        string targetUserEmail,
        CancellationToken cancellationToken)
    {
        var request = _requestCreator.RequestWithContentAndToken(
            $"/relationship/send-friendship-request?targetUserEmail={targetUserEmail}",
            "POST",
            token,
            targetUserEmail);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return;
        }

        var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
        throw new ApiClientException(errorMessage);
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

        if (response.StatusCode != HttpStatusCode.OK)
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
    /// Return list of users who you've send request.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of users. Every user is UserDto which contains: Id, Email.</returns>
    public async Task<IReadOnlyList<UserDto>> GetRequestsFromMeAsync(string token, CancellationToken cancellationToken)
    {
        var request =
            _requestCreator.RequestWithToken(
                "/relationship/get-requests-from-me",
                "GET",
                token);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
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
    /// Return list of users who have send request to you.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>List of users. Every user is UserDto which contains: Id, Email.</returns>
    public async Task<IReadOnlyList<UserDto>> GetRequestsToMeAsync(string token, CancellationToken cancellationToken)
    {
        var request =
            _requestCreator.RequestWithToken(
                "/relationship/get-requests-to-me",
                "GET",
                token);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
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
    /// Accept friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="requestedEmail">An email of user who you want to accept.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    public async Task AcceptFriendshipRequestAsync(
        string token,
        string requestedEmail,
        CancellationToken cancellationToken)
    {
        var request = _requestCreator.RequestWithToken(
            $"/relationship/accept-friendship-request?requestedEmail={requestedEmail}",
            "PATCH",
            token);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return;
        }

        var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
        throw new ApiClientException(errorMessage);
    }

    /// <summary>
    /// Reject friendship request if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user.</param>
    /// <param name="requestedEmail">An email of user who you want to reject.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    public async Task RejectFriendshipRequestAsync(
        string token,
        string requestedEmail,
        CancellationToken cancellationToken)
    {
        var request = _requestCreator.RequestWithToken(
            $"/relationship/reject-friendship-request?requestedEmail={requestedEmail}",
            "PATCH",
            token);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return;
        }

        var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
        throw new ApiClientException(errorMessage);
    }
}