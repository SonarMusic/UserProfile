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
        Console.WriteLine(t);
        userClient.LogoutAsync(t.ToString());
        Console.WriteLine(userClient.GetAsync("CCDFBE51-69D8-4FC0-9D3A-D37B3E5FBE79").Result.Password);
    }
}