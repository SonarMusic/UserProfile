using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static void Main()
    {
        var httpClient = new HttpClient();
        var apiClient = new UserApiClient("https://localhost:7062", httpClient);

        try
        {
            var t = apiClient.GetAsync(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJuYW1laWQiOiIxZmIxNTJhNy0zNjBhLTRkMzQtOWZhZC1hZDYxMTA4MmM0NDMiLCJuYmYiOjE2NTUyMzIxMTUsImV4cCI6MTY1NTgzNjkxNSwiaWF0IjoxNjU1MjMyMTE1LCJpc3MiOiJTb25hci5Vc2VyUHJvZmlsZSIsImF1ZCI6IlNvbmFyIn0.sjgcGT8wrjCvDaZxpXcIzBO4KwVO8IVSfJeF-pYyO1A"
                , CancellationToken.None);

            Console.WriteLine(t.Result.Email);
            Console.WriteLine(t.Result.Id);
        }
        catch (ApiClientException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}