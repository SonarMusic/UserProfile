namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static void Main()
    {
        var httpClient = new HttpClient();
        var userClient = new UserApiClient("https://localhost:7062/", httpClient);

        var t = userClient.LoginAsync(new UserLoginDto
        {
            Id = Guid.Parse("FD58D9DA-6925-4859-B9D7-744F35AB6155"),
            Password = "string"
        }).Result;
        userClient.SetToken(t.ToString()).LogoutAsync();
        Console.WriteLine(userClient.SetToken("CCDFBE51-69D8-4FC0-9D3A-D37B3E5FBE79").GetAsync().Result.Password);
    }
}