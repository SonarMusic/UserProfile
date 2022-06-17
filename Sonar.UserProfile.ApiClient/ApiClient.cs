using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient;

public class UserApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public UserApiClient(string baseUrl, HttpClient httpClient)
    {
        BaseUrl = baseUrl;
        _httpClient = httpClient;
    }

    public string BaseUrl { get; set; }

    /// <summary>
    /// Create new user with new token which will expire in 7 days.
    /// </summary>
    /// <param name="userRegisterDto">DTO which contains parameters for new user: email, password.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>New token.</returns>
    /// <exception cref="ApiClientException">Throws if error status code appears.</exception>
    public async Task<string> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken)
    {
        var request = CreateContentRequest("/user/register", "POST", userRegisterDto);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        var errorMessage = await CreateErrorMessage(response, cancellationToken);
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
        var request = CreateContentRequest("/user/login", "PATCH", userLoginDto);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        var errorMessage = await CreateErrorMessage(response, cancellationToken);
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
        var request = CreateTokenRequest("/user/get", "GET", token);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            var responseDeserialized = JsonSerializer.Deserialize<UserGetDto>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return responseDeserialized!;
        }

        var errorMessage = await CreateErrorMessage(response, cancellationToken);
        throw new ApiClientException(errorMessage);
    }

    /// <summary>
    /// Add a friend to user if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user..</param>
    /// <param name="friendEmail">An email if friend who you want to add.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    public async Task AddFriendAsync(string token, string friendEmail, CancellationToken cancellationToken)
    {
        var request = CreateContentAndTokenRequest("/user/add-friend", "PATCH", token, friendEmail);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return;
        }

        var errorMessage = await CreateErrorMessage(response, cancellationToken);
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
        var request = CreateTokenRequest("/user/get-friends", "GET", token);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            var responseDeserialized = JsonSerializer.Deserialize<IReadOnlyList<UserGetDto>>(responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return responseDeserialized!;
        }

        var errorMessage = await CreateErrorMessage(response, cancellationToken);
        throw new ApiClientException(errorMessage);
    }

    private HttpRequestMessage CreateContentRequest(string route, string restVerb, object content)
    {
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{BaseUrl}{route}");
        request.Method = new HttpMethod(restVerb);
        request.Content = JsonContent.Create(content);

        return request;
    }

    private HttpRequestMessage CreateTokenRequest(string route, string restVerb, string token)
    {
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{BaseUrl}{route}");
        request.Method = new HttpMethod(restVerb);
        request.Headers.Add("Token", token);

        return request;
    }

    private HttpRequestMessage CreateContentAndTokenRequest(string route, string restVerb, string token, object content)
    {
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{BaseUrl}{route}");
        request.Method = new HttpMethod(restVerb);
        request.Headers.Add("Token", token);
        request.Content = JsonContent.Create(content);

        return request;
    }

    private async Task<string> CreateErrorMessage(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.Append("Status code: ");
        messageBuilder.Append(response.StatusCode);

        /*if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            return messageBuilder.ToString();
        }*/

        messageBuilder.Append(". Message: ");
        messageBuilder.Append(await response.Content.ReadAsStringAsync(cancellationToken));

        return messageBuilder.ToString();
    }
}