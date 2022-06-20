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
    
    public async Task AddFriend(Guid userId, string friendEmail, CancellationToken cancellationToken)
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

    public Task<IReadOnlyList<User>> GetFriendsById(Guid userId, CancellationToken cancellationToken)
    {
        return _relationshipRepository.GetFriendsByIdAsync(userId, cancellationToken);
    }
}