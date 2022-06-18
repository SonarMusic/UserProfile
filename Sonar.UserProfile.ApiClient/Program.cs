using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static async Task Main()
    {
        var httpClient = new HttpClient();
        var apiClient = new UserApiClient("https://localhost:7062", httpClient);


        try
        {
            var token1 = apiClient.RegisterAsync(
                new UserRegisterDto { Email = "a10@a.a", Password = "cvsbva" },
                CancellationToken.None).Result;
            
            var token2 = apiClient.RegisterAsync(
                new UserRegisterDto { Email = "b10@b.b", Password = "cvsbva" },
                CancellationToken.None).Result;

            await apiClient.AddFriendAsync(token1, "b10@b.b", CancellationToken.None);

            var friends1 = apiClient.GetFriendsAsync(token1, CancellationToken.None);
            var friends2 = apiClient.GetFriendsAsync(token2, CancellationToken.None);

            Console.WriteLine(friends1.Result[0].Email);
            Console.WriteLine(friends2.Result[0].Email);
        }
        catch (ApiClientException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}