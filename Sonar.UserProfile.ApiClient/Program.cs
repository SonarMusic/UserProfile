namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static void Main()
    {
        var httpClient = new HttpClient();
        var userClient = new UserApiClient("https://localhost:7062/", httpClient);

        Console.WriteLine(userClient.SetToken("CCDFBE51-69D8-4FC0-9D3A-D37B3E5FBE79").GetAsync().Result.Password);
    }
}