using Sonar.UserProfile.Core.Domain.Exceptions;
using Sonar.UserProfile.Core.Domain.Users.Repositories;
using Sonar.UserProfile.Core.Domain.Users.Services.Interfaces;
using Sonar.UserProfile.Core.Domain.Users.ValueObjects;

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

    public async Task SendFriendshipRequestAsync(
        Guid userId,
        string targetUserEmail,
        CancellationToken cancellationToken)
    {
        var dataBaseUser = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (dataBaseUser.Email == targetUserEmail)
        {
            throw new InvalidRequestException("Users must be different.");
        }

        var dataBaseFriend = await _userRepository.GetByEmailAsync(targetUserEmail, cancellationToken);

        if (await _relationshipRepository.IsRelationshipAsync(
                userId,
                dataBaseFriend.Id,
                RelationshipStatus.Friends,
                cancellationToken))
        {
            throw new DataOccupiedException("These users are already friends.");
        }
        
        if (await _relationshipRepository.IsRelationshipAsync(
                userId,
                dataBaseFriend.Id,
                RelationshipStatus.Request,
                cancellationToken))
        {
            throw new DataOccupiedException("You already send request.");
        }

        await _relationshipRepository.SendFriendshipRequestAsync(userId, dataBaseFriend.Id, cancellationToken);
    }

    public Task<IReadOnlyList<User>> GetRelationshipsAsync(
        Guid userId,
        RelationshipStatus relationshipStatus,
        CancellationToken cancellationToken)
    {
        return _relationshipRepository.GetRelationshipsAsync(userId, relationshipStatus, cancellationToken);
    }

    public async Task AcceptFriendshipRequestAsync(Guid userId, string requestedEmail,
        CancellationToken cancellationToken)
    {
        var requested = await _userRepository.GetByEmailAsync(requestedEmail, cancellationToken);

        if (!await _relationshipRepository.IsRelationshipAsync(
                userId,
                requested.Id,
                RelationshipStatus.Request,
                cancellationToken))
        {
            throw new NotFoundException("This user didn't request friendship from you.");
        }

        await _relationshipRepository.ChangeRelationshipStatusAsync(
            userId,
            requested.Id,
            RelationshipStatus.Friends,
            cancellationToken);
    }

    public async Task RejectFriendshipRequestAsync(Guid userId, string requestedEmail,
        CancellationToken cancellationToken)
    {
        var requested = await _userRepository.GetByEmailAsync(requestedEmail, cancellationToken);

        if (!await _relationshipRepository.IsRelationshipAsync(
                userId,
                requested.Id,
                RelationshipStatus.Request,
                cancellationToken))
        {
            throw new NotFoundException("This user didn't request friendship from you.");
        }

        await _relationshipRepository.ChangeRelationshipStatusAsync(
            userId,
            requested.Id,
            RelationshipStatus.Reject,
            cancellationToken);
    }
}