namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static void Main()
    {
        var httpClient = new HttpClient();
        var userClient = new UserApiClient("https://localhost:7062/", httpClient); 
        userClient.RegisterAsync(new UserRegisterDto { Email = "string", Password = "c" }).GetAwaiter().GetResult();
        userClient.LoginAsync(new UserLoginDto { Email = "d", Password = "c" }).GetAwaiter().GetResult();
    }
}