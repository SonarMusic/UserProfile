using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient;

public class UserApiClient : IApiClient
{
    private string _baseUrl = "";
    private readonly HttpClient _httpClient;

    public UserApiClient(string baseUrl, HttpClient httpClient)
    {
        BaseUrl = baseUrl;
        _httpClient = httpClient;
    }

    public string BaseUrl
    {
        get => _baseUrl;
        set => _baseUrl = value;
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
        var urlBuilder = new StringBuilder();
        urlBuilder.Append(_baseUrl).Append("/User/register");

        var url = urlBuilder.ToString();

        var content = JsonContent.Create(userRegisterDto);
        var response = await _httpClient.PostAsync(url, content, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        var messageBuilder = new StringBuilder();
        messageBuilder.Append("Status code: ");
        messageBuilder.Append(response.StatusCode);

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new ApiClientException(messageBuilder.ToString());
        }

        messageBuilder.Append(". Message: ");
        messageBuilder.Append(await response.Content.ReadAsStringAsync(cancellationToken));

        throw new ApiClientException(messageBuilder.ToString());
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
        var urlBuilder = new StringBuilder();
        urlBuilder.Append(_baseUrl).Append("/User/login");

        var url = urlBuilder.ToString();

        var content = JsonContent.Create(userLoginDto);
        var response = await _httpClient.PatchAsync(url, content, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        var messageBuilder = new StringBuilder();
        messageBuilder.Append("Status code: ");
        messageBuilder.Append(response.StatusCode);

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new ApiClientException(messageBuilder.ToString());
        }

        messageBuilder.Append(". Message: ");
        messageBuilder.Append(await response.Content.ReadAsStringAsync(cancellationToken));

        throw new ApiClientException(messageBuilder.ToString());
    }

    /// <summary>
    /// Return a user model if token hasn't expired yet.
    /// </summary>
    /// <param name="tokenHeader">Contains token.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>User model which contains: ID, email.</returns>
    /// <exception cref="ApiClientException">Throws if error status code appears.</exception>
    public async Task<UserGetDto> GetAsync(
        [FromHeader(Name = "Token")] string tokenHeader,
        CancellationToken cancellationToken)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append(_baseUrl).Append("/User/get");

        var url = urlBuilder.ToString();
            
        _httpClient.DefaultRequestHeaders.Add("Token", tokenHeader);
        var response = await _httpClient.GetAsync(url, cancellationToken);
        _httpClient.DefaultRequestHeaders.Clear();

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            var responseDeserialized = JsonSerializer.Deserialize<UserGetDto>(responseString);
            return responseDeserialized!;
        }

        var messageBuilder = new StringBuilder();
        messageBuilder.Append("Status code: ");
        messageBuilder.Append(response.StatusCode);

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new ApiClientException(messageBuilder.ToString());
        }

        messageBuilder.Append(". Message: ");
        messageBuilder.Append(await response.Content.ReadAsStringAsync(cancellationToken));

        throw new ApiClientException(messageBuilder.ToString());
    }
}