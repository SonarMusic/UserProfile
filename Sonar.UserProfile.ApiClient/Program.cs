using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static void Main()
    {
        var httpClient = new HttpClient();
        var apiClient = new UserApiClient("https://localhost:7062", httpClient);

        var token = apiClient.RegisterAsync(new UserRegisterDto
        {
            Email = "cwq@v.r",
            Password = "abcsdvsdv"
        }, CancellationToken.None).Result;
        
        try
        {
            var t = apiClient.GetAsync(token, CancellationToken.None);

            Console.WriteLine(t.Result.Email);
            Console.WriteLine(t.Result.Id);
        }
        catch (ApiClientException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}