﻿namespace Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;

public interface IRelationshipService
{
    Task SendFriendshipRequestAsync(Guid userId, string targetUserEmail, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUserFriendsAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetRequestsFromUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetRequestsToUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> IsFriends(Guid leftUserId, Guid rightUserId, CancellationToken cancellationToken);
    Task AcceptFriendshipRequestAsync(Guid userId, string requestedEmail, CancellationToken cancellationToken);
    Task RejectFriendshipRequestAsync(Guid userId, string requestedEmail, CancellationToken cancellationToken);
    Task BanUser(Guid userId, string requestedEmail, CancellationToken cancellationToken);
    Task UnbanUser(Guid userId, string requestedEmail, CancellationToken cancellationToken);
    Task Unfriend(Guid userId, string requestedEmail, CancellationToken cancellationToken);
    
}