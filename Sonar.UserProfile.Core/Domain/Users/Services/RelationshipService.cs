using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;

namespace Sonar.UserProfile.Core.Domain.Users.Services;

public class RelationshipService : IRelationshipService
{
    private readonly IUserRepository _userRepository;
    private readonly IRelationshipRepository _relationshipRepository;

    public RelationshipService(IUserRepository userRepository, IRelationshipRepository relationshipRepository)
    {
        _userRepository = userRepository;
        _relationshipRepository = relationshipRepository;
    }
    
    public async Task AddFriendAsync(Guid userId, string friendEmail, CancellationToken cancellationToken)
    {
        var dataBaseUser = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (dataBaseUser.Email == friendEmail)
        {
            throw new InvalidRequestException("Users must be different.");
        }
        var dataBaseFriend = await _userRepository.GetByEmailAsync(friendEmail, cancellationToken);

        if (await _relationshipRepository.IsFriendsAsync(userId, dataBaseFriend.Id, cancellationToken))
        {
            throw new DataOccupiedException("These users are already friends.");
        }

        await _relationshipRepository.SendFriendshipRequestAsync(userId, dataBaseFriend.Id, cancellationToken);
    }

    public Task<IReadOnlyList<User>> GetFriendsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _relationshipRepository.GetFriendsAsync(userId, cancellationToken);
    }

    public Task<IReadOnlyList<User>> GetRequestedAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _relationshipRepository.GetRequestedAsync(userId, cancellationToken);
    }

    public Task<IReadOnlyList<User>> GetRejectedAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _relationshipRepository.GetRejectedAsync(userId, cancellationToken);
    }

    public Task AcceptFriendshipRequestAsync(Guid userId, Guid requestedId, CancellationToken cancellationToken)
    {
        return _relationshipRepository.AcceptFriendshipRequestAsync(userId, requestedId, cancellationToken);
    }

    public Task RejectFriendshipRequestAsync(Guid userId, Guid requestedId, CancellationToken cancellationToken)
    {
        return _relationshipRepository.RejectFriendshipRequestAsync(userId, requestedId, cancellationToken);
    }
}