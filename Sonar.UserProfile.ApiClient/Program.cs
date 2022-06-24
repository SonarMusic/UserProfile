using Sonar.UserProfile.ApiClient.Dto;

namespace Sonar.UserProfile.ApiClient;

public static class Program
{
    public static async Task Main()
    {
        // TODO: Когда-нибудь будут нормальные тесты.
        
        var httpClient = new HttpClient();
        var userApiClient = new UserApiClient("https://localhost:7062", httpClient);
        var relationshipApiClient = new RelationshipApiClient("https://localhost:7062", httpClient);


        var token1 = userApiClient.RegisterAsync(
            new UserAuthDto { Email = "a5@a.a", Password = "string" },
            CancellationToken.None).Result;

        var token2 = userApiClient.RegisterAsync(
            new UserAuthDto { Email = "b5@b.b", Password = "string" },
            CancellationToken.None).Result;

        var token3 = userApiClient.RegisterAsync(
            new UserAuthDto { Email = "c5@c.c", Password = "string" },
            CancellationToken.None).Result;

        await relationshipApiClient.SendFriendshipRequestAsync(token1, "b5@b.b", CancellationToken.None);
        await relationshipApiClient.SendFriendshipRequestAsync(token1, "c5@c.c", CancellationToken.None);

        var request1 =
            relationshipApiClient.GetRequestsFromMeAsync(token1, CancellationToken.None);
        var request2 =
            relationshipApiClient.GetRequestsToMeAsync(token2, CancellationToken.None);
        var request3 =
            relationshipApiClient.GetRequestsToMeAsync(token3, CancellationToken.None);

        Console.WriteLine(request1.Result.Count);
        Console.WriteLine(request2.Result[0].Email);
        Console.WriteLine(request3.Result[0].Email);

        await relationshipApiClient.AcceptFriendshipRequestAsync(token2, "a5@a.a", CancellationToken.None);
        await relationshipApiClient.RejectFriendshipRequestAsync(token3, "a5@a.a", CancellationToken.None);

        var friends1 =
            relationshipApiClient.GetFriendsAsync(token1, CancellationToken.None);
        var friends2 =
            relationshipApiClient.GetFriendsAsync(token2, CancellationToken.None);
        var friends3 =
            relationshipApiClient.GetFriendsAsync(token3, CancellationToken.None);

        Console.WriteLine(friends1.Result[0].Email);
        Console.WriteLine(friends2.Result[0].Email);
        Console.WriteLine(friends3.Result.Count);

        request1 =
            relationshipApiClient.GetRequestsToMeAsync(token1, CancellationToken.None);
        request2 =
            relationshipApiClient.GetRequestsToMeAsync(token2, CancellationToken.None);
        request3 =
            relationshipApiClient.GetRequestsToMeAsync(token3, CancellationToken.None);

        Console.WriteLine(request1.Result.Count);
        Console.WriteLine(request2.Result.Count);
        Console.WriteLine(request3.Result.Count);

        request1 =
            relationshipApiClient.GetRequestsFromMeAsync(token1, CancellationToken.None);
        request2 =
            relationshipApiClient.GetRequestsFromMeAsync(token2, CancellationToken.None);
        request3 =
            relationshipApiClient.GetRequestsFromMeAsync(token3, CancellationToken.None);

        Console.WriteLine(request1.Result.Count);
        Console.WriteLine(request2.Result.Count);
        Console.WriteLine(request3.Result.Count);
    }
}