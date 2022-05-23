using System.Text;

namespace Sonar.UserProfile.ApiClient;

public partial interface IUserApiClient
{
    IUserApiClient SetToken(string token);
}

public partial class UserApiClient
{
    private string _token;
    
    private Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
        
    private Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder url, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_token) && !request.Headers.Contains("Token")) request.Headers.Add("Token", _token);
        return Task.CompletedTask;
    }
        
    private Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string url, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_token) && !request.Headers.Contains("Token")) request.Headers.Add("Token", _token);
        return Task.CompletedTask;
    }

    public IUserApiClient SetToken(string token)
    {
        _token = token;
        return this;
    }
}