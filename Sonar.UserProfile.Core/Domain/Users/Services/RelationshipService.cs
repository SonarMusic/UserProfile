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

        var dataBaseTarget = await _userRepository.GetByEmailAsync(targetUserEmail, cancellationToken);

        if (await _relationshipRepository.CheckStatusAsync(userId, dataBaseTarget.Id, cancellationToken)
                is RelationshipStatus.Friends ||
            await _relationshipRepository.CheckStatusAsync(dataBaseTarget.Id, userId, cancellationToken)
                is RelationshipStatus.Friends)
        {
            throw new DataOccupiedException("These users are already friends.");
        }

        if (await _relationshipRepository.CheckStatusAsync(userId, dataBaseTarget.Id, cancellationToken)
                is RelationshipStatus.Request ||
            await _relationshipRepository.CheckStatusAsync(dataBaseTarget.Id, userId, cancellationToken)
                is RelationshipStatus.Request)
        {
            throw new DataOccupiedException("There are already a request between these users.");
        }

        await _relationshipRepository.SendFriendshipRequestAsync(userId, dataBaseTarget.Id, cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetUserFriendsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var friends = (await _relationshipRepository.GetUsersInRelationshipFromUserAsync(
            userId,
            RelationshipStatus.Friends,
            cancellationToken)).ToList();

        friends.AddRange(await _relationshipRepository.GetUsersInRelationshipToUserAsync(
            userId,
            RelationshipStatus.Friends,
            cancellationToken));

        return friends;
    }

    public Task<IReadOnlyList<User>> GetRequestsFromUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _relationshipRepository.GetUsersInRelationshipFromUserAsync(
            userId,
            RelationshipStatus.Request,
            cancellationToken);
    }

    public Task<IReadOnlyList<User>> GetRequestsToUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _relationshipRepository.GetUsersInRelationshipToUserAsync(
            userId,
            RelationshipStatus.Request,
            cancellationToken);
    }

    public async Task AcceptFriendshipRequestAsync(Guid userId, string requestedEmail,
        CancellationToken cancellationToken)
    {
        var requested = await _userRepository.GetByEmailAsync(requestedEmail, cancellationToken);

        if (await _relationshipRepository.CheckStatusAsync(requested.Id, userId, cancellationToken)
            is not RelationshipStatus.Request)
        {
            throw new NotFoundException("This user didn't request friendship from you.");
        }

        await _relationshipRepository.UpdateStatusAsync(
            requested.Id,
            userId,
            RelationshipStatus.Friends,
            cancellationToken);
    }

    public async Task RejectFriendshipRequestAsync(
        Guid userId,
        string requestedEmail,
        CancellationToken cancellationToken)
    {
        var requested = await _userRepository.GetByEmailAsync(requestedEmail, cancellationToken);

        if (await _relationshipRepository.CheckStatusAsync(requested.Id, userId, cancellationToken)
            is not RelationshipStatus.Request)
        {
            throw new NotFoundException("This user didn't request friendship from you.");
        }

        await _relationshipRepository.DeleteAsync(
            requested.Id,
            userId,
            cancellationToken);
    }
}