namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static void Main()
    {
        var httpClient = new HttpClient();
        var userClient = new UserApiClient("https://localhost:7062/", httpClient);
        userClient.LoginAsync(new UserLoginDto { Email = "d", Password = "c" }).GetAwaiter().GetResult();
    }
}