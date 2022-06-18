using System.Net;
using System.Text.Json;
using Sonar.UserProfile.ApiClient.Interfaces;
using Sonar.UserProfile.ApiClient.Tools;
using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient;

public class ApiClientUser : IApiClientUser
{
    private readonly HttpClient _httpClient;
    private readonly RequestCreator _requestCreator;

    public ApiClientUser(string baseUrl, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _requestCreator = new RequestCreator(baseUrl);
    }

    /// <summary>
    /// Create new user with new token which will expire in 7 days.
    /// </summary>
    /// <param name="userRegisterDto">DTO which contains parameters for new user: email, password.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>New token.</returns>
    /// <exception cref="ApiClientException">Throws if error status code appears.</exception>
    public async Task<string> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken)
    {
        var request = _requestCreator.RequestWithContent("/user/register", "POST", userRegisterDto);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
        throw new ApiClientException(errorMessage);
    }

    /// <summary>
    /// Generate new token to user if password matched. Token will expire in 7 days.
    /// </summary>
    /// <param name="userLoginDto">DTO which contains parameters to identify user: email, password</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>New token.</returns>
    /// <exception cref="ApiClientException">Throws if error status code appears.</exception>
    public async Task<string> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken)
    {
        var request = _requestCreator.RequestWithContent("/user/login", "PATCH", userLoginDto);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
        throw new ApiClientException(errorMessage);
    }

    /// <summary>
    /// Return a user model if token hasn't expired yet.
    /// </summary>
    /// <param name="token">User token.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>User model which contains: ID, email.</returns>
    /// <exception cref="ApiClientException">Throws if error status code appears.</exception>
    public async Task<UserGetDto> GetAsync(string token, CancellationToken cancellationToken)
    {
        var request = _requestCreator.RequestWithToken("/user/get", "GET", token);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errorMessage = await _requestCreator.ErrorMessage(response, cancellationToken);
            throw new ApiClientException(errorMessage);
        }

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseDeserialized = JsonSerializer.Deserialize<UserGetDto>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return responseDeserialized!;
    }
}