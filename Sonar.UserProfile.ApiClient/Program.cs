using Sonar.UserProfile.ApiClient.Tools;
using Sonar.UserProfile.Web.Controllers.Users.Dto;

namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static async Task Main()
    {
        var httpClient = new HttpClient();
        var userApiClient = new UserApiClient("https://localhost:7062", httpClient);
        var relationshipApiClient = new RelationshipApiClient("https://localhost:7062", httpClient);


        try
        {
            var token1 = userApiClient.RegisterAsync(
                new UserRegisterDto { Email = "a1@a.a", Password = "cvsbva" },
                CancellationToken.None).Result;
            
            var token2 = userApiClient.RegisterAsync(
                new UserRegisterDto { Email = "b1@b.b", Password = "cvsbva" },
                CancellationToken.None).Result;

            await relationshipApiClient.AddFriendAsync(token1, "b1@b.b", CancellationToken.None);

            var friends1 = relationshipApiClient.GetFriendsAsync(token1, CancellationToken.None);
            var friends2 = relationshipApiClient.GetFriendsAsync(token2, CancellationToken.None);

            Console.WriteLine(friends1.Result[0].Email);
            Console.WriteLine(friends2.Result[0].Email);
        }
        catch (ApiClientException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}