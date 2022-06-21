using Sonar.UserProfile.ApiClient.Dto;
using Sonar.UserProfile.ApiClient.Tools;
using Sonar.UserProfile.ApiClient.ValueObjects;

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
                new UserRegisterDto { Email = "a5@a.a", Password = "cvsbva" },
                CancellationToken.None).Result;

            var token2 = userApiClient.RegisterAsync(
                new UserRegisterDto { Email = "b5@b.b", Password = "cvsbva" },
                CancellationToken.None).Result;

            await relationshipApiClient.SendFriendshipRequestAsync(token1, "b5@b.b", CancellationToken.None);

            var request1 =
                relationshipApiClient.GetRelationshipsAsync(token1, RelationshipStatus.Request, CancellationToken.None);
            var request2 =
                relationshipApiClient.GetRelationshipsAsync(token2, RelationshipStatus.Request, CancellationToken.None);

            Console.WriteLine(request1.Result[0].Email);
            Console.WriteLine(request2.Result[0].Email);

            await relationshipApiClient.AcceptFriendshipRequestAsync(token2, "a5@a.a", CancellationToken.None);

            var friends1 =
                relationshipApiClient.GetRelationshipsAsync(token1, RelationshipStatus.Friends, CancellationToken.None);
            var friends2 =
                relationshipApiClient.GetRelationshipsAsync(token2, RelationshipStatus.Friends, CancellationToken.None);
            
            Console.WriteLine(friends1.Result[0].Email);
            Console.WriteLine(friends2.Result[0].Email);
        }
        catch (ApiClientException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}