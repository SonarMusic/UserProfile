namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static async Task Main()
    {
        var httpClient = new HttpClient();
        var userApiClient = new UserApiClient("https://localhost:7062", httpClient);

        var token = await userApiClient.LoginByDiscordBotAsync(
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMDEwMTAxMDExMDEwMTAxMDEwMTAxMDEwMTAxMCIsIm5hbWUiOiJOaWNlIEJvdCBGb3IgRGlzY29yZCIsImlhdCI6MTY1NjI1NDg1OX0.ugQP-wM4qXeF-54GHgtFABmlaPNUKWQ3RfBhKQjd33k",
            "user@example.com", CancellationToken.None);

        var user = await userApiClient.GetAsync(token, CancellationToken.None);
        
        Console.WriteLine(user.Email);
        Console.WriteLine(user.Id);
        Console.WriteLine(user.AccountType);
    }
}