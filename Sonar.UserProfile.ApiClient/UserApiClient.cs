using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sonar.UserProfile.ApiClient.Dto;
using Sonar.UserProfile.ApiClient.Interfaces;
using Sonar.UserProfile.ApiClient.Tools;

namespace Sonar.UserProfile.ApiClient;

public class UserApiClient : IUserApiClient
{
    private readonly HttpClient _httpClient;
    private readonly RequestCreator _requestCreator;

    public UserApiClient(string baseUrl, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _requestCreator = new RequestCreator(baseUrl);
    }

    /// <summary>
    /// Return a user model if token hasn't expired yet.
    /// </summary>
    /// <param name="token">User token.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>User model which contains: ID, email, AccountType.</returns>
    public async Task<UserDto> GetAsync(string token, CancellationToken cancellationToken)
    {
        var request = _requestCreator.RequestWithToken("/user/get", "GET", token);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode is not HttpStatusCode.OK)
        {
            var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
            throw new ApiClientException(errorMessage);
        }

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseDeserialized = JsonSerializer.Deserialize<UserDto>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        });
        return responseDeserialized!;
    }
}