using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Sonar.UserProfile.ApiClient.Tools;

public class RequestCreator
{
    public RequestCreator(string baseUrl)
    {
        BaseUrl = baseUrl;
    }
    
    public string BaseUrl { get; }
    
    public HttpRequestMessage RequestWithContent(string route, string restVerb, object content)
    {
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{BaseUrl}{route}");
        request.Method = new HttpMethod(restVerb);
        request.Content = JsonContent.Create(content);

        return request;
    }

    public HttpRequestMessage RequestWithToken(string route, string restVerb, string token)
    {
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{BaseUrl}{route}");
        request.Method = new HttpMethod(restVerb);
        request.Headers.Add("Token", token);

        return request;
    }

    public HttpRequestMessage RequestWithContentAndToken(
        string route,
        string restVerb,
        string token,
        object content)
    {
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{BaseUrl}{route}");
        request.Method = new HttpMethod(restVerb);
        request.Headers.Add("Token", token);
        request.Content = JsonContent.Create(content);

        return request;
    }
    
    public async Task<string> ErrorMessage(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.Append("Status code: ");
        messageBuilder.Append(response.StatusCode);

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            return messageBuilder.ToString();
        }

        messageBuilder.Append(". Message: ");
        messageBuilder.Append(await response.Content.ReadAsStringAsync(cancellationToken));

        return messageBuilder.ToString();
    }
}